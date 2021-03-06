using System;
using System.IO;


namespace Nez.Persistence.Binary {
	public class FileDataStore {
		public enum FileFormat {
			Binary,
			Text
		}

		private FileFormat _fileFormat;
		private string _persistentDataPath;
		private ReuseableBinaryWriter _writer;
		private ReuseableBinaryReader _reader;

		public FileDataStore() : this(null) { }


		/// <summary>
		/// Initializes a new instance of the <see cref="T:Nez.Persistence.Binary.FileDataStore"/> class. If <paramref name="persistentDataPath"/>
		/// is null, it will use Utils.GetStorageRoot().
		/// </summary>
		/// <param name="persistentDataPath">Persistent data path.</param>
		/// <param name="fileFormat">File format.</param>
		public FileDataStore(string persistentDataPath, FileFormat fileFormat = FileFormat.Binary) {
			if (persistentDataPath == null) {
				string exeName = Path.GetFileNameWithoutExtension(AppDomain.CurrentDomain.FriendlyName).Replace(".vshost", "");
				persistentDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), exeName);
			}

			// ensure directory exists
			Directory.CreateDirectory(persistentDataPath);
			_persistentDataPath = persistentDataPath;
			_fileFormat = fileFormat;
			if (_fileFormat == FileFormat.Binary) {
				// if we are binary, cache our reader/writer since they are reuseable
				_writer = new ReuseableBinaryWriter();
				_reader = new ReuseableBinaryReader();
			}
		}

		#region DataStore

		public void Save(string filename, IPersistable persistable) {
			string tmpFile = GetTmpFile(filename);
			using (IPersistableWriter writer = GetDataWriter(tmpFile)) {
				writer.Write(persistable);
			}

			File.Copy(tmpFile, Path.Combine(_persistentDataPath, filename), true);
			File.Delete(tmpFile);
		}

		public void Load(string filename, IPersistable persistable) {
			string file = Path.Combine(_persistentDataPath, filename);
			if (File.Exists(file)) {
				using (IPersistableReader reader = GetDataReader(file)) {
					reader.ReadPersistableInto(persistable);
				}
			}
		}

		public void Clear() {
			DirectoryInfo di = new DirectoryInfo(_persistentDataPath);
			foreach (FileInfo file in di.GetFiles()) {
				file.Delete();
			}
		}

		#endregion

		private IPersistableWriter GetDataWriter(string file) {
			if (_fileFormat == FileFormat.Binary) {
				_writer.SetStream(file);
				return _writer;
			}

			return new TextPersistableWriter(file);
		}

		private IPersistableReader GetDataReader(string file) {
			if (_fileFormat == FileFormat.Binary) {
				_reader.SetStream(file);
				return _reader;
			}

			return new TextPersistableReader(file);
		}

		private string GetTmpFile(string filename) {
			return Path.Combine(_persistentDataPath, filename + ".tmp");
		}
	}
}