//?? bu kodda optimizasyon yok 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class twoDimensionalAnimationStateController : MonoBehaviour
{
    Animator animator;
    float velocityX = 0.0f;
    float velocityZ = 0.0f;
    public float acceleration = 2.0f;
    public float deceleration = 2.0f;

    public float maximumWalkVelocity = 0.5f;
    public float maximumRunVelocity = 2.0f;

    void Start()
    {
        //* search the gameObject this script is attached to and get the animator component
        animator = GetComponent<Animator>();
        
    }

    void Update()
    {
        //* Input will be true if the player is pressing on the passed in key parameters
        //* Get key input from player
        bool forwardPressed = Input.GetKey("w");
        bool leftPressed = Input.GetKey("a");
        bool rightPressed = Input.GetKey("d");
        bool runPressed = Input.GetKey("left shift");

        //* set current MaxVelocity
        float currentMaxVelocity = runPressed ? maximumRunVelocity : maximumWalkVelocity;

        //* if player presses forward, increase velocity in z direction
        // if(forwardPressed && velocityZ < 0.05f && !runPressed) 
        if(forwardPressed && velocityZ < currentMaxVelocity)
        {
            velocityZ += Time.deltaTime * acceleration;
        }

        //* increase velocity in left direction
        // if(leftPressed && velocityX > -0.05f && !runPressed)
        if(leftPressed && velocityX > -currentMaxVelocity)
        {
            velocityX -= Time.deltaTime * acceleration;
        }

        //* increase velocity in right direction
        // if(rightPressed && velocityX < 0.05f && !runPressed)
        if(rightPressed && velocityX < currentMaxVelocity)
        {
            velocityX += Time.deltaTime * acceleration;
        }

        //* decrase velocity
        if(!forwardPressed && velocityZ > 0.0f)
        {
            velocityZ -= Time.deltaTime * deceleration;
        }

        //* reset velocity
        if(!forwardPressed && velocityZ < 0.0f)
        {
            velocityZ = 0.0f;
        }

        //* increase velocityX if left is not pressed and velocityX < 0
        if(!leftPressed && velocityX < 0.0f)
        {
            velocityX += Time.deltaTime * deceleration;
        }

        //* decrease velocityX if right is not pressed and velocityX > 0
        if(!rightPressed && velocityX > 0.0f)
        {
            velocityX -= Time.deltaTime * deceleration;
        }

        //* reset velocityX
        if(!leftPressed && !rightPressed && velocityX != 0.0f  && (velocityX > -0.05f && velocityX < 0.05f))
        {
            velocityX = 0.0f;
        }

        //* lock forward (bu kısımda tam 2'nin üstünde değer ise tam 2'ye sabitliyoruz)
        if(forwardPressed && runPressed && velocityZ > currentMaxVelocity)
        {
            velocityZ = currentMaxVelocity;
        }
        //* decelerate to the maximum walk velocity
        else if (forwardPressed && velocityZ > currentMaxVelocity)
        {
            velocityZ -= Time.deltaTime * deceleration;
            //* round to the currentMaxVelocity if within offset
            if(velocityZ > currentMaxVelocity && velocityZ <(currentMaxVelocity + 0.05f))
            {
                velocityZ = currentMaxVelocity;
            }
        }
        //* round to the currentMaxVelocity if within offset ( bu kısımda küçük sapmalar bile olsa direk 0.5f'e sabit olmasını sağlıyoruz)
        else if (forwardPressed && velocityZ < currentMaxVelocity && velocityZ > (currentMaxVelocity - 0.05f))
        {
            velocityZ = currentMaxVelocity;
        }

        //* locking left
        if(leftPressed && runPressed && velocityX < -currentMaxVelocity)
        {
            velocityX = -currentMaxVelocity;
        }
        //* decelerate to the maximum left velocity
        else if (leftPressed && velocityX < -currentMaxVelocity)
        {
            velocityX += Time.deltaTime * deceleration;
            //* round to the currentMaxVelocity if within offset
            if(velocityX < -currentMaxVelocity && velocityX >(-currentMaxVelocity -0.05f))
            {
                velocityX = -currentMaxVelocity;
            }
        }
        //* round to the currentMaxVelocity if within offset
        else if (leftPressed && velocityX > -currentMaxVelocity && velocityX < (-currentMaxVelocity + 0.05f))
        {
            velocityX = -currentMaxVelocity;
        }

        //* locking right
        if(rightPressed && runPressed && velocityX > currentMaxVelocity)
        {
            velocityX = currentMaxVelocity;
        }
        //* decelerate to the maximum right velocity
        else if (rightPressed && velocityX > currentMaxVelocity)
        {
            velocityX -= Time.deltaTime * deceleration;
            //* round to the currentMaxVelocity if within offset
            if(velocityX > currentMaxVelocity && velocityX <(currentMaxVelocity + 0.05f))
            {
                velocityX = currentMaxVelocity;
            }
        }
        //* round to the currentMaxVelocity if within offset
        else if (rightPressed && velocityX < currentMaxVelocity && velocityX > (currentMaxVelocity -0.05f))
        {
            velocityX = currentMaxVelocity;
        }


        
        
        //* set the parameter to our local variables values
        animator.SetFloat("VelocityZ",velocityZ);
        animator.SetFloat("VelocityX",velocityX);
    }
}
