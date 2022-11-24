using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CharacterState { Walk, Attack, Slide, Jump, Shield, Hit, Dead }
public class CharacterController : ScalingManagement
{
    public static CharacterController Instance;

    private CharacterState state = CharacterState.Walk;

    [SerializeField] Animator anim;

    [SerializeField] float slideTime = 1.5f;
    float slideTimer;

    [SerializeField] Transform character;
    [SerializeField] float jumpTime;
    float jumpTimer;
    [SerializeField] float jumpSpeed = 2f;
    Vector3 originalPos;
    [SerializeField] float shieldTime = 2.5f;
    float shieldTimer;

    [SerializeField] int startLevel = 1;
    int level { get; set; }
    public int Level { get { return level; } }

    int expRequired;
    int currentExp;

    bool jumped = false;

    [SerializeField] int defaultAttackDamage = 5;
    public int currentAttackDamage { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void Init()
    {
        if (PlayerPrefs.HasKey("CharacterLevel"))
        {
            level = PlayerPrefs.GetInt("CharacterLevel");
        }
        else
        {
            level = 1;
        }
        originalPos = character.position;
        currentAttackDamage = defaultAttackDamage;
        GameController.Instance.SetLevelText();
        GameController.Instance.ChangeMaxHealth();
        state = CharacterState.Walk;

        expRequired = ExpRequiredToLevelUp(level);
        if (PlayerPrefs.HasKey("currentExp"))
        {
            currentExp = PlayerPrefs.GetInt("currentExp");
        }
        //GameController.Instance.IncreaseMaxHealth(MaxHealthIncrease(Level));
    }

    private void Start()
    {
        Init();
        
    }

    private void Update()
    {
        if (state == CharacterState.Slide)
        {
            slideTimer += Time.deltaTime;

            if (slideTimer >= slideTime)
            {
                state = CharacterState.Walk;
                anim.Play("Warrior_Walk");
                slideTimer = 0;
            }
        }

        if (state == CharacterState.Shield)
        {
            shieldTimer += Time.deltaTime;

            if (shieldTimer >= shieldTime)
            {
                state = CharacterState.Walk;
                anim.Play("Warrior_Walk");
                shieldTimer = 0;
            }
        }

        if (jumped)
        {
            if (jumpTimer < jumpTime)
            {
                character.position = Vector3.MoveTowards(character.position, new Vector3(originalPos.x, originalPos.y + .75f, 0), jumpSpeed * Time.deltaTime);
            }
            jumpTimer += Time.deltaTime;
            if (jumpTimer >= jumpTime)
            {
                StartCoroutine(FallAfterJump());
                character.position = Vector3.MoveTowards(character.position, new Vector3(originalPos.x, originalPos.y, 0), (jumpSpeed * .8f) * Time.deltaTime);
            }
        }

        if (state == CharacterState.Jump)
        {
            jumped = true;
        }
    }

    private IEnumerator FallAfterJump()
    {
        CharacterController.Instance.PlayAnim("Warrior_JumpDown");
        yield return new WaitForSeconds(0.33f);
        state = CharacterState.Walk;
        anim.Play("Warrior_Walk");
        jumped = false;
        jumpTimer = 0;
    }

    public void PlayAnim(string animName)
    {
        anim.Play(animName);

        if (animName == "Warrior_Slide")
        {
            state = CharacterState.Slide;
        }
        else if (animName == "Warrior_Slash")
        {
            state = CharacterState.Attack;
        }
        else if (animName == "Warrior_Shield")
        {
            state = CharacterState.Shield;
            GameController.Instance.SetScrollType(ScrollBackground.Static);
        }
        else if (animName == "Warrior_Jump")
        {
            state = CharacterState.Jump;
        }
        else if (animName == "Warrior_TakeHit")
        {
            state = CharacterState.Hit;
        }
    }

    public CharacterState GetCharacterState()
    {
        return state;
    }

    public void SetCharacterState(CharacterState state)
    {
        this.state = state;
    }

    public int CurrentAttackDamage
    {
        get { return currentAttackDamage+level; }
    } 

    public void GainExp(int exp)
    {
        GameController.Instance.AddToExp(exp);
        currentExp += exp;
        if (currentExp >= expRequired)
        {
            MusicPlayer.Instance.PlaySoundEffectByName("PlayerLevelUp2");
            level++;
            PlayerPrefs.SetInt("CharacterLevel", level);
            GameController.Instance.LeveledUp();
            int tempExp = currentExp - expRequired;
            currentExp = tempExp;
            expRequired = ExpRequiredToLevelUp(level);
            //GameController.Instance.IncreaseMaxHealth(MaxHealthIncrease(Level));
            int lives = GetHealth(level);
            GameController.Instance.ChangeMaxHealth();
        }
        PlayerPrefs.SetInt("currentExp", currentExp);
    }
}
