
namespace Ships.Action
{
    public class ActionResult
    {
        public readonly ActionStatus  Status;
        public readonly string Messege;
        public readonly bool AllowRepeat;

        public ActionResult(ActionStatus status, string messege, bool allowRepeat)
        {
            Status = status;
            Messege = messege;
            AllowRepeat = allowRepeat;
        }


        public override string ToString()
        {
            return $"Action Result: Status: `{Status}`  Message: `{Messege}` , AllowReapet`{AllowRepeat}`";
        }
    }
}