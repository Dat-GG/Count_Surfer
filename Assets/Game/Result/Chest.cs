using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Random = UnityEngine.Random;

namespace Funzilla
{
    public class Chest : MonoBehaviour
    {
        [SerializeField] private Text txtGem;
        internal Text TxtGem => txtGem;
        [SerializeField] public Button chestButton;
        [SerializeField] public Image chestImage;
        [SerializeField] private GameObject gemPrefab;
        [SerializeField] private Transform endPointFlyGem;
        internal bool isOpened = false;
        private Sequence _sequenTween;
        private Sequence _sequenOpenedTween;
        private Sequence _sequenFlyGemTween;

        private void Awake()
        {
            txtGem.gameObject.SetActive(false);
            chestButton.onClick.AddListener(OnChestClick);
        }

        private void Start()
        {
            _sequenTween = DOTween.Sequence();
            _sequenTween.Append(chestImage.transform.DOShakeScale(1, 0.2f, 10, 10));
            _sequenTween.PrependInterval(3);
            _sequenTween.SetLoops(-1);
            
        }

        private void OnChestClick()
        {
            isOpened = true;
            _sequenTween.Kill();
            _sequenOpenedTween.Append(chestImage.transform.DOShakePosition(1f, 10f, 10, 90, true, false).OnComplete(OnOpenedChest));

        }

        private  void OnOpenedChest()
        {
            Profile.Instance.CoinAmount += Convert.ToInt32(txtGem.text);
            txtGem.gameObject.SetActive(true);
            chestImage.gameObject.SetActive(false);
            _sequenOpenedTween.Kill();
            GemFlyAnimation();
        }

        private void GemFlyAnimation()
        {
            var gemInChest = Convert.ToInt32(txtGem.text);
            for (int i = 0; i < gemInChest; i++)
            {
                Vector3 posFly = new Vector3(RandomFlyGemPositon(), RandomFlyGemPositon(), 0);
                var gem = Instantiate(gemPrefab, transform);
                gem.transform.position = transform.position;
                Destroy(gem,1);
                _sequenFlyGemTween.Append(gem.transform.DOLocalMove(posFly, 0.1f));
                _sequenFlyGemTween.Append(gem.transform.DOMove(endPointFlyGem.position, 0.5f).SetDelay(0.5f));
                _sequenFlyGemTween.Kill();
            }
        }

        private int RandomFlyGemPositon()
        {
            return Random.Range(-300, 300);
        }

        
    }
}
