using UnityEngine;
using System.Collections;
 
public class AmbientSound : MonoBehaviour 
{
    [SerializeField]
    private AudioSource audioSource;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            audioSource.Play();
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if (other.CompareTag("Player"))
        {
            audioSource.Stop();
        }
    }
}