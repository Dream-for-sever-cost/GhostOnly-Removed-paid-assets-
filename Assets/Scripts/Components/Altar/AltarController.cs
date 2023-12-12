using Data;
using DG.Tweening;
using System.Collections;
using UnityEngine;
using Util;

public class AltarController : MonoBehaviour, IDamagable
{
    [SerializeField] private SpriteRenderer _alterRenderer;
    [SerializeField] private Sprite[] alterImages;
    [SerializeField] private SpriteRenderer _effectRenderer;
    [SerializeField] private GameObject _damagePanel;

    [SerializeField] private Animator _devoteAnimator;

    private SoulManager soulManager;
    private GameManager gameManager;
    private TargetManager targetManager;
    private InteractNPC interactNPC;
    private Sequence _damageSequence;

    private void Awake()
    {
        interactNPC = GetComponent<InteractNPC>();    
    }

    private void Start()
    {      
        soulManager = Managers.Soul;
        gameManager = Managers.GameManager;
        targetManager = Managers.Target;

        targetManager.AddSlave(transform, TargetManager.TargetType.Alter);

        interactNPC.EventInteract.AddListener(FillSoul);    
    }

    public void FillSoul()
    {
        if (!soulManager.CheckSoul(Constants.GameSystem.AlterDevote * 10))
        {
            Managers.Sound.PlaySound(Data.SoundType.AltarLack);
            return;
        }

        soulManager.UseSoul(Constants.GameSystem.AlterDevote * 10);
        gameManager.currentAlter += Constants.GameSystem.AlterDevote;

        if (IsDeath())
        {
            DOTween.KillAll(true);
            GetResultData();
            gameManager.GameClear();
        }

        SetAlterImage(gameManager.currentAlter);
        _devoteAnimator.SetTrigger(Constants.AniParams.Devote);
        Managers.Sound.PlaySound(SoundType.AltarInput);
    }

    public bool IsDeath() => (Constants.GameSystem.MaxAlter <= gameManager.currentAlter);

    public void TakeDamage(float damage)
    {
        gameManager.currentAlter -= (int)(damage / 2);
        StartCoroutine(CoSequence());

        if (gameManager.currentAlter <= 0)
        {
            gameManager.currentAlter = 0;
            DOTween.KillAll(true);
            GetResultData();
            gameManager.GameOver();

            //Managers.Sound.PlaySound(Data.SoundType.AltarHit, transform.position);
        }

        SetAlterImage(gameManager.currentAlter);
        Managers.Sound.PlaySound(SoundType.AltarHit, Vector2.zero, true);
    }

    private void SetAlterImage(int hp)
    {
        switch (hp)
        {
            case <= Constants.GameSystem.AlterLow:
                _alterRenderer.sprite = alterImages[0];
                _effectRenderer.color = Constants.Colors.AlterLow;
                break;

            case > Constants.GameSystem.AlterLow and <= Constants.GameSystem.AlterMid:
                _alterRenderer.sprite = alterImages[1];
                _effectRenderer.color = Constants.Colors.AlterMid;
                break;

            default:
                _alterRenderer.sprite = alterImages[2];
                _effectRenderer.color = Color.white;
                break;
        }
    }

    private void GetResultData()
    {
        Managers.GameManager.currentDay = DayManager.Instance.CurrentDay;
        Managers.GameManager.realTime = DayManager.Instance.RealTime;
    }

    private void DamageSequence()
    {
        _damageSequence = DOTween.Sequence()
            .OnStart(() =>
            {
                _damagePanel.GetComponent<CanvasGroup>().alpha = 0f;
            })
            .Prepend(_damagePanel.GetComponent<CanvasGroup>().DOFade(0.12f, 0.125f))
            .Append(_damagePanel.GetComponent<CanvasGroup>().DOFade(0f, 0.125f));   
    }

    private IEnumerator CoSequence()
    {
        _damagePanel.SetActive(true);
        DamageSequence();
        yield return new WaitForSeconds(0.25f);
        _damageSequence.Kill();
        _damagePanel.SetActive(false);
    }
}
