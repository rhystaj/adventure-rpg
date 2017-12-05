using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using UnityEngine.UI;

/**
 * A utility class of static methods to be used in cooroutines to perform simple animations over time.
 */ 
public class OverTime
{

    /**
     * Move the object with the given transform, between 2 given points, over the given time.
     */
    public static IEnumerator Move(Transform transform, Vector3 startPosition, Vector3 endPosition, float seconds, Action onCompletion)
    {

        float totalTime = 0;
        while (totalTime < seconds)
        {

            transform.localPosition = Vector3.Lerp(startPosition, endPosition, totalTime / seconds);
            totalTime += Time.deltaTime;
            yield return null;

        }

        if (onCompletion != null)
            onCompletion();

        yield return null;

    }

    /**
     * Move the object with the given transform, between 2 given points, over the given time, stopping at the given percantage of the distance.
     */
    public static IEnumerator Move(Transform transform, Vector3 startPosition, Vector3 endPosition, float seconds, float stopAtPercentage, Action onCompletion)
    {

        float totalTime = 0;
        while (totalTime < seconds)
        {

            transform.localPosition = Vector3.Lerp(startPosition, endPosition, totalTime / seconds);
            totalTime += Time.deltaTime;

            if (totalTime / seconds >= stopAtPercentage) break;
            yield return null;

        }

        if (onCompletion != null)
            onCompletion();

        yield return null;

    }

    /**
     * Translate the object with the given transform, by the given distance, over the given time.
     */
    public static IEnumerator Translate(Transform transform, Vector3 distance, float seconds, Action onCompletion)
    {

        float totalTime = 0;

        while (totalTime < seconds)
        {
            float timeSince = Time.deltaTime;
            transform.Translate(
                distance.x * (timeSince / seconds),
                distance.y * (timeSince / seconds),
                distance.z * (timeSince / seconds)
            );
            totalTime += timeSince;
            yield return null;

        }

        if (onCompletion != null)
            onCompletion();

        yield return null;

    }


    public static IEnumerator Rotate(Transform transform, Vector3 eulerRotation, float seconds, Action onCompletion)
    {

        float totalTime = 0;
        Vector3 totalRotation = Vector3.zero;
        Vector3 beginningRotation = transform.localEulerAngles;

        while (totalTime < seconds &&
             (Mathf.Abs(totalRotation.x) <= Mathf.Abs(eulerRotation.x) &&
              Mathf.Abs(totalRotation.y) <= Mathf.Abs(eulerRotation.y) &&
              Mathf.Abs(totalRotation.z) <= Mathf.Abs(eulerRotation.z)))
        {
            float timeSince = Time.deltaTime;

            Vector3 rotationAmount = new Vector3(
                eulerRotation.x * (timeSince / seconds),
                eulerRotation.y * (timeSince / seconds),
                eulerRotation.z * (timeSince / seconds)
            );

            transform.Rotate(rotationAmount);

            totalTime += timeSince;
            totalRotation += rotationAmount;

            yield return null;
        }

        //Adjusts the rotation to where it should be in case the loop under or over compensated.
        transform.localEulerAngles = beginningRotation;
        transform.Rotate(eulerRotation);


        if (onCompletion != null)
            onCompletion();

        yield return null;

    }

    public static IEnumerator ChangeOpacity(Image image, float startAlpha, float endAlpha, float duration, Action onCompletion)
    {

        image.color = new Color(image.color.r, image.color.g, image.color.b, startAlpha);

        image.CrossFadeColor(new Color(image.color.r, image.color.g, image.color.b, endAlpha), duration, false, true);
        yield return new WaitForSeconds(duration);

        if (onCompletion != null) onCompletion();

    }

    public static IEnumerator FadeVolume(AudioSource sound, float duration)
    {

        float totalTime = 0;
        float volStart = sound.volume;

        while (totalTime < duration)
        {
            sound.volume = Mathf.Lerp(volStart, 0, totalTime / duration);
            totalTime += Time.deltaTime;

            yield return null;
        }

    }

}
