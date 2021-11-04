using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TypewriterEffect : MonoBehaviour
{
    [SerializeField] private float typewriterSpeed = 40f;

    public AudioSource source;
    public AudioClip clip;


    public Coroutine Run(string textToType, TMP_Text textLabel)
    {
        return StartCoroutine(TypeText(textToType, textLabel));
    }

    private IEnumerator TypeText(string textToType, TMP_Text textLabel)
    {
        textLabel.text = string.Empty;
        //yield return new WaitForSeconds(1);

        float t = 0;
        int charIndex = 0;
        int index = 0;

        while (charIndex < textToType.Length)
        {
            t += Time.deltaTime * typewriterSpeed;
            charIndex = Mathf.FloorToInt(t);
            charIndex = Mathf.Clamp(charIndex, 0, textToType.Length);

            textLabel.text = textToType.Substring(0, charIndex);

            if (charIndex == index)
            {
                source.PlayOneShot(clip);
                index++;
            }

            yield return null;
        }

        textLabel.text = textToType;
    }


}
