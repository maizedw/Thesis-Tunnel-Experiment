using System.Collections.Generic;
using LSL;
using UnityEngine;

public class LSLController : MonoBehaviour {
    /*
     * This is a simple example of an LSL Outlet to stream out irregular events occurring in Unity.
     * This uses only LSL.cs and is intentionally simple. For a more robust version, see another sample.
     *
     * We stream out the trigger event during OnTriggerEnter which is, in our opinion, the closest
     * time to when the trigger actually occurs (i.e., independent of its rendering).
     * A simple way to print the events is with pylsl: `python -m pylsl.examples.ReceiveStringMarkers`
     *
     * If you are instead trying to log a stimulus event then there are better options. Please see the
     * LSL4Unity SimpleStimulusEvent Sample for such a design.
     */
    string StreamName = "TunnelExperiment";
    private string[] substreams = { "Footsteps", "Spline", "Control" }; 
    string StreamType = "Markers";
    private Dictionary<string, StreamOutlet> outlets = new Dictionary<string, StreamOutlet>();
    private string[] sample = { "" };
    
    public static LSLController Instance;

    void Awake() {
        Instance = this;
    }

    void Start() {
        var hash = new Hash128();
        hash.Append(StreamName);
        hash.Append(StreamType);
        hash.Append(gameObject.GetInstanceID());
        foreach (var substream in substreams) {
            StreamInfo streamInfo = new StreamInfo($"{StreamName}.{substream}", StreamType, 1, LSL.LSL.IRREGULAR_RATE,
                channel_format_t.cf_string, hash.ToString());
            outlets.Add(substream, new StreamOutlet(streamInfo));
        }
    }

    private void Log(StreamOutlet outlet, string message) {
        if (outlet == null) return;
        
        Debug.Log($"<color=yellow>Outlet: {outlet.info().name()}. Message: {message}</color>");
        
        sample[0] = message;
        // Debug.Log(sample[0]);
        outlet.push_sample(sample);
    }

    public void LogFootsteps(string message) {
        Log(outlets["Footsteps"], message);
    }
    
    public void LogSpline(string message) {
        Log(outlets["Spline"], message);
    }
    
    public void LogControl(string message) {
        Log(outlets["Control"], message);
    }
}