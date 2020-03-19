using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSegment
{
    public List<int> spawns = new List<int>();
    public int waitTime;
}

public class Wave : MonoBehaviour
{
    
    [TextArea(3,10)]
    public string pattern;

    public WaveSegment[] PatternToWaveSegments()
    {
        //wave pattern = 0 0 1 -> ultimul nr e timpule de asteptare si restul sunt indici pt monstrii
        //EX : 1 2 3 1 2 -> 2 este waiting time si restul sunt indici

        //cut up the lines in the pattern
        string [] lines = pattern.Split('\n');

        //create a list which will be return
        List<WaveSegment> segments = new List<WaveSegment>();

        //loop thorugh lines of patterns <=> each wave
        foreach(string line in lines)
        {
            WaveSegment segment = new WaveSegment();

            //cut the lines into spawns and time
            string [] spawns = line.Split(' ');

            //loop throught the spawn numbers for enemy and ignore the time
            for(int i = 0; i < spawns.Length - 1;i++)
            {
                segment.spawns.Add(int.Parse(spawns[i]));
            }

            // add the last number in the segment as the wait time
            segment.waitTime = int.Parse(spawns[spawns.Length - 1]);

            //add segment to the wave
            segments.Add(segment);
        }

        //return wave
        return segments.ToArray();
    }
}
