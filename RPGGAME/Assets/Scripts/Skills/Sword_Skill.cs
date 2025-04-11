using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;
using UnityEngine.UI;

public enum SwordType
{
    Regular,
    Bounce,
    pierce,
    Spin
}

public class Sword_Skill : Skill
{
    public SwordType swordType = SwordType.Regular;

    [Header("Bounce info")] 
    [SerializeField] private UI_SkillTreeSlot bounceUnlockButton;
    [SerializeField] private int bounceAmount;
    [SerializeField] private float bounceGravity;
    [SerializeField] private float bounceSpeed;
        
    
    [Header("Pierce info")]
    [SerializeField] private UI_SkillTreeSlot pierceUnlockButton;
    [SerializeField] private int Pierceamount;
    [SerializeField] private float pierceGravity;

    [Header("Spin info")]
    [SerializeField] private UI_SkillTreeSlot spinUnlockButton;
    [SerializeField] private float maxTravelDistance = 7;
    [SerializeField] private float spinDuration = 2;
    [SerializeField] private float spinGravity = 1;
    [SerializeField] private float hitCooldewn = .35f;



    [Header("Skill info")] [SerializeField]
    private UI_SkillTreeSlot swordUnlockButton;
    public bool swordUnlocked { get; private set; }
    [SerializeField] private GameObject swordPrefab;
    [FormerlySerializedAs("launchDir")] [SerializeField] private Vector2 launchForce;
    [SerializeField] private float swordGravity;
    [SerializeField] private float freezeTimeDuration;
    [SerializeField] private float returnSpeed;

    private Vector2 finalDir;

    [Header("Aim dots")] 
    [SerializeField] private int numberOfDots;
    [SerializeField] private float spaceBetweenDots;
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private Transform dotsParent;
    
    
    private GameObject[] dots;

    [Header("Passive skills")] [SerializeField]
    private UI_SkillTreeSlot timeStopUnlockButton;

    public bool timeStopUnlocked { get; private set; }
    [SerializeField] private UI_SkillTreeSlot volnurableUnlockButton;
    public bool volnurableUnlocked { get; private set; }




    protected override void Start()
    {
        base.Start();
        
        GenerateDots();

        SetupGravity();
        
        swordUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockSwords);
        bounceUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockBounce);
        pierceUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockPierce);
        spinUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockSpins);
        timeStopUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockTimeStop);
        volnurableUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockVolnurable);
    }

    private void SetupGravity()
    {
        if (swordType == SwordType.Bounce)
            swordGravity = bounceGravity;
        else if (swordType == SwordType.pierce)
            swordGravity = pierceGravity;
        else if (swordType == SwordType.Spin)
            swordGravity = spinGravity;
    }

    protected override void Update()
    {
        if(Input.GetKeyUp(KeyCode.Mouse1))
            finalDir = new Vector2(AimDirection().normalized.x * launchForce.x, AimDirection().normalized.y * launchForce.y);

        if (Input.GetKey(KeyCode.Mouse1))
        {
            for (int i = 0; i < dots.Length; i++)
            {
                dots[i].transform.position =  DotsPosition(i * spaceBetweenDots);
            }
        }
    }

    #region MyRegion

    protected override void CheckUnlock()
    {
        UnlockSwords();
        UnlockBounce();
        UnlockPierce();
        UnlockSpins();
        UnlockTimeStop();
        UnlockVolnurable();
        
        
    }

    private void UnlockTimeStop()
    {
        if (timeStopUnlockButton.unlocked)
            timeStopUnlocked = true;
    }

    private void UnlockVolnurable()
    {
        if (volnurableUnlockButton.unlocked)
            volnurableUnlocked = true;
    }

    private void UnlockSwords()
    {
        if (swordUnlockButton.unlocked)
        {
            swordType = SwordType.Regular;
            swordUnlocked = true;
        }

        
    }

    private void UnlockSpins()
    {
        if (spinUnlockButton.unlocked)
            swordType = SwordType.Spin;
    }

    private void UnlockBounce()
    {
        if(bounceUnlockButton.unlocked)
            swordType = SwordType.Bounce;
    }

    private void UnlockPierce()
    {
        if(pierceUnlockButton.unlocked)
            swordType = SwordType.pierce;
    }


    #endregion

    public void CreateSword()
    {
        GameObject newSword = Instantiate(swordPrefab, player.transform.position, transform.rotation);
        Sword_Skill_Controller newSwordScript = newSword.GetComponent<Sword_Skill_Controller>();

        if (swordType == SwordType.Bounce)
            newSwordScript.SetupBounce(true, bounceAmount, bounceSpeed);
        else if (swordType == SwordType.pierce)
            newSwordScript.SetupPierce(Pierceamount);
        else if (swordType == SwordType.Spin)
            newSwordScript.SetupSpin(true, maxTravelDistance, spinDuration, hitCooldewn);
        
            
            
        
        
        newSwordScript.SetupSword(finalDir, swordGravity, player, freezeTimeDuration, returnSpeed);
        player.AssignNewSword(newSword);
        DotsActive(false);
    }

    #region Aim region

    public Vector2 AimDirection()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - playerPosition;
        return direction;
        
    }

    public void DotsActive(bool _isActive)
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].SetActive(_isActive);
        }
    }

    private void GenerateDots()
    {
        dots = new GameObject[numberOfDots];
        for (int i = 0; i < numberOfDots; i++)
        {
            dots[i] = Instantiate(dotPrefab, player.transform.position, Quaternion.identity, dotsParent);
            dots[i].SetActive(false);
        }
    }

    private Vector2 DotsPosition(float t)
    {
        Vector2 position = (Vector2)player.transform.position + new Vector2(AimDirection().normalized.x * launchForce.x, AimDirection().normalized.y * launchForce.y) * t + .5f * (Physics2D.gravity * swordGravity) * (t * t);
        return position;
    }
    #endregion

}
