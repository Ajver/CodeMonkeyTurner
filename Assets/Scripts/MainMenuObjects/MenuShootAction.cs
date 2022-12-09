using System;
using UnityEngine;

public class MenuShootAction: MonoBehaviour
{
    [SerializeField] private BulletProjectile bulletProjectilePrefab;
    [SerializeField] private Transform shootPointTransform;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource aimAudio;
    [SerializeField] private AudioSource shootAudio;
    
    private enum State
    {
        Aiming,
        Shooting,
    }
    
    private State state;
    private float stateTimer;
    private bool isActive;

    private IDamageable damageableTarget;
    private bool canShootBullet;

    private Action OnShootingFinished;

    public void TakeAction(IDamageable shootTarget, Action completeCallback)
    {
        damageableTarget = shootTarget;
        OnShootingFinished = completeCallback;
        
        aimAudio.Play();
        
        state = State.Aiming;
        float aimingStateTime = 0.6f;
        stateTimer = aimingStateTime;
        isActive = true;
    }
    
    public void Update()
    {
        if (!isActive)
        {
            return;
        }

        switch (state)
        {
            case State.Aiming:
                Aim();
                break;
            case State.Shooting:
                TryShoot();
                break;
        }

        stateTimer -= Time.deltaTime;
        if (stateTimer <= 0f)
        {
            NextState();
        }
    }
    
    private void Aim()
    {
        Vector3 faceDirection = (damageableTarget.GetTransform().position - transform.position).normalized;

        float rotationSpeed = 10f;
        transform.forward = Vector3.Lerp(transform.forward, faceDirection, rotationSpeed * Time.deltaTime);
    }

    private void TryShoot()
    {
        if (canShootBullet)
        {
            canShootBullet = false;
            Shoot();
        }
    }
    
    private void Shoot()
    {
        BulletProjectile bullet = Instantiate(bulletProjectilePrefab, shootPointTransform.position, Quaternion.identity);
        
        Vector3 targetPosition = damageableTarget.GetTransform().position;
        targetPosition.y = shootPointTransform.position.y;
        bullet.Setup(targetPosition);
        
        damageableTarget.Damage(100);
        
        animator.SetTrigger("Shoot");
        
        shootAudio.Play();
        
        ScreenShake.Instance.Shake(1f);
    }

    private void NextState()
    {
        switch (state)
        {
            case State.Aiming:
                state = State.Shooting;
                float shootingStateTime = 0.1f;
                stateTimer = shootingStateTime;
                canShootBullet = true;
                break;
            case State.Shooting:
                OnShootingFinished();
                isActive = false;
                break;
        }
    }
}