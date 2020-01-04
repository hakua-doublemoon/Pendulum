//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class Pole : MonoBehaviour
{
    const float kk = 0.09F;
    const int controll_period = 1000 / 50;

    float desired_angle  = 300;
    float check_angle = 270;
    int start_time = 0;
    int delta_mill_sec = 0;

    // Start is called before the first frame update
    void Start()
    {
        desired_angle = curr_angle_get() + 90 + 90;
        check_angle = curr_angle_get() + 180 * 0.63F;
        start_time = curr_time_get();
        delta_mill_sec = 0;
    }

    // Update is called once per frame
    void Update()
    {
        delta_mill_sec += ellapse_time_get();
        if (delta_mill_sec < controll_period) {
            return;
        }
        delta_mill_sec = 0;
        //Debug.Log(delta_mill_sec + "passed");

        float angle_1 = curr_angle_get();
        //Debug.Log(angle_1);
        //File.AppendAllText(@"test.txt", curr_time_get() + "," + angle_1 + Environment.NewLine);
        /*
        if (curr_time_get() - start_time > 1000) {
            spin_motor(0, true);
            return;
        } 
        //*/

        float delta_angle = desired_angle - angle_1;
        //float delta_angle = desired_angle;
        float input_speed = delta_angle / (controll_period/1000F);
        spin_motor((int)(input_speed*kk), false);

        //Debug.Log(input_speed);
        if (angle_1 > check_angle) {
            check_angle = 3600;
            Debug.Log(curr_time_get() - start_time);
        }
    }

    private void spin_motor(int velocity, bool is_release)
    {
        HingeJoint2D hinge = this.GetComponent<HingeJoint2D>();
        JointMotor2D motor = hinge.motor;
        motor.motorSpeed = velocity;
        hinge.motor = motor;

        if (is_release) {
            hinge.useMotor = false;
        }
    }

    private int ellapse_time_get()
    {
        return (int)(Time.deltaTime*1000);
    }

    private int curr_time_get()
    {
        return DateTime.Now.Hour * 60 * 60 * 1000
           + DateTime.Now.Minute * 60 * 1000
           + DateTime.Now.Second * 1000
           + DateTime.Now.Millisecond;
    }

    private float curr_angle_get()
    {
        HingeJoint2D hinge = this.GetComponent<HingeJoint2D>();
        return hinge.jointAngle;
    }
}
