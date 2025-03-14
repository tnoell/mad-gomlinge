using UnityEngine;

public class Tutorial_EngineSparking : SequenceElement
{
    private Module engine;
    private bool mTaskDone;

    private void MTaskDone()
    {
        mTaskDone = true;
    }

    protected override void SubBegin()
    {
        mTaskDone = false;
        engine = GameObject.FindWithTag("TutorialStartEngine").GetComponent<Module>();
        MaintenanceTimer timer = engine.GetComponent<MaintenanceTimer>();
        timer.CompleteMaintenance();
        timer.ChangeProgress(0.5f);
        MaintenanceTask mTask =  engine.GetComponent<MaintenanceTask>();
        mTask.onCompleted.AddListener(MTaskDone);
        onFinish.AddListener(() => mTask.onCompleted.RemoveListener(MTaskDone));
    }

    protected override bool SubIsFinished()
    {
        return mTaskDone;
    }
}
