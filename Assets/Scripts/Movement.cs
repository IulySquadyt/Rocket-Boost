using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] InputAction thrust;
    [SerializeField] InputAction rotation;
    [SerializeField] float thrustForce = 10f;
    [SerializeField] float rotationSpeed = 100f;
    [SerializeField] AudioClip thrustSound;
    [SerializeField] ParticleSystem mainJetParticles;
    [SerializeField] ParticleSystem leftJetParticles;
    [SerializeField] ParticleSystem rightJetParticles;

    Rigidbody rb;
    AudioSource audioSource;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        thrust.Enable();
        rotation.Enable();

    }
    void FixedUpdate()
    {
        ProcessThrust();
        ProcessRotation();
    }

    private void ProcessThrust()
    {
        if (thrust.IsPressed())
        {
            StartThrusting();
        }
        else
        {
            StopThrusting();
        }
    }

    private void StopThrusting()
    {
        audioSource.Stop();
        mainJetParticles.Stop();
    }

    private void StartThrusting()
    {
        Debug.Log("Thrusting!");
        rb.AddRelativeForce(Vector3.up * thrustForce * Time.fixedDeltaTime);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(thrustSound);
        }
        if (!mainJetParticles.isPlaying)
        {
            mainJetParticles.Play();
        }
    }

    private void ProcessRotation()
    {
        float rotationInput = rotation.ReadValue<float>();
        if (rotationInput < 0)
        {
            StartRotation(rotationSpeed, rightJetParticles);
        }
        else if (rotationInput > 0)
        {
            StartRotation(-rotationSpeed, leftJetParticles);
        }
        else
        {
            StopRotating();
        }
    }

    private void StopRotating()
    {
        rightJetParticles.Stop();
        leftJetParticles.Stop();
    }

    private void StartRotation(float speed, ParticleSystem particles)
    {
        ApplyRotation(speed);
        if (!particles.isPlaying)
        {
            particles.Stop();
            particles.Play();
        }
    }

    private void ApplyRotation(float rotationThisFrame)
    {
        rb.freezeRotation = true;
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.fixedDeltaTime);
        rb.freezeRotation = false;
    }
}
