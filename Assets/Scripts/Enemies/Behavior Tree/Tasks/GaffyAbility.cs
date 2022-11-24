using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class GaffyAbility : EnemyAction
{
    [SerializeField] Enemy enemy;

    private bool specialFinished;

    private float specialTime;
    private float specialTimer;

    private float spawnTime;
    private float spawnTimer;    

    [SerializeField] string special;
    bool animPlayed = false;

    [SerializeField] GameObject bombPrefab;

    public override void OnStart()
    {
        if (enemy == null)
        {
            enemy = GetComponent<Enemy>();
        }

        specialTime = enemy.GetSpecialAnimTime();
        spawnTime = specialTime / 22;
    }

    public override TaskStatus OnUpdate()
    {
        if (GameController.Instance.GetGameType() == GameType.Game)
        {
            if (!animPlayed)
            {
                MusicPlayer.Instance.PlaySoundEffectByName("GaffySpecial");
                animPlayed = true;
                anim.Play(special);
            }

            spawnTimer += Time.deltaTime;
            if (spawnTimer > spawnTime)
            {
                GameController.Instance.SpawnButton(bombPrefab, 0);
                spawnTimer = 0;
            }

            specialTimer += Time.deltaTime;
            if (specialTimer > specialTime)
            {
                specialFinished = true;
            }

            return specialFinished ? TaskStatus.Success : TaskStatus.Running;
        }

        return TaskStatus.Running;
    }

    public override void OnEnd()
    {
        specialFinished = false;
        specialTimer = 0f;
        spawnTimer = 0;
        animPlayed = false;
    }
}
