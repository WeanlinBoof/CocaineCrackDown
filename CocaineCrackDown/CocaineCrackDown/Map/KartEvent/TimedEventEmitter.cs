using Nez;

namespace CocaineCrackDown.Entiteter.Events {

    public class TimedEventEmitter : KartEventSändare {

        private readonly float IntervalMininum;

        private readonly float MaxInterval;

        private int Upprepa;

        public TimedEventEmitter(Karta map , string eventKey , float intervalMininum , float Maxinterval , int upprepa) : base(map , eventKey) {
            IntervalMininum = intervalMininum;
            MaxInterval = Maxinterval;
            Upprepa = upprepa;
        }

        public override void OnAddedToScene() {
            base.OnAddedToScene();
            ScheduleNextEmit(initialSchedule: true);
        }

        private void ScheduleNextEmit(bool initialSchedule = false) {
            if(!initialSchedule) {
                EmitMapEvent();
            }

            if(Upprepa > 0) {
                Upprepa--;
            }
            else if(Upprepa == 0) {
                return;
            }

            float timeSeconds = Random.Range(IntervalMininum , MaxInterval);
            Core.Schedule(timeSeconds , _ => ScheduleNextEmit());
        }
    }
}