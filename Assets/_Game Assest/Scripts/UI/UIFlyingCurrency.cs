using System.Collections;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class UIFlyingCurrency : MonoBehaviour
{
    [SerializeField] private GameObject flyingCurrency;

    public void CurrencyCollected(int currencyIncrease = 1, int maxCount = 1)
    {
        CurrencyCollected(new Vector3(Screen.width * .5f, Screen.height * .5f), currencyIncrease, maxCount);
    }

    public void CurrencyCollected(Vector3 startPosition, int currencyIncrease = 1, int maxCount = 1)
    {
        StartCoroutine(InternalProcess());

        IEnumerator InternalProcess()
        {
            var interval = new WaitForSeconds(.1f);

            if (currencyIncrease < 5f)
            {
                for (var i = 0; i < currencyIncrease; i++)
                {
                    FlyCurrency(startPosition, 1);

                    yield return interval;
                }
            }
            else
            {
                var divided = Mathf.FloorToInt(currencyIncrease / (float)maxCount);
                var remaining = currencyIncrease % maxCount;

                for (var i = 0; i < maxCount; i++)
                {
                    FlyCurrency(startPosition, divided);

                    yield return interval;
                }

                FlyCurrency(startPosition, remaining);
            }
        }
    }

    private void FlyCurrency(Vector3 startPosition, int currencyIncrease)
    {
        const int flightTime = 1;

        var currentCurrency = Instantiate(flyingCurrency, transform).transform;

        currentCurrency.gameObject.SetActive(true);
        currentCurrency.position = startPosition + Random.onUnitSphere * (Screen.width * .15f);
        currentCurrency.localScale = Vector3.zero;

        var endPosition = flyingCurrency.transform.position;
        var midPosition = Vector3.Lerp(startPosition, endPosition, Random.Range(.3f, .7f));
        midPosition += Random.onUnitSphere * (Screen.width * .15f);

        DOTween.Sequence()
            .Append(currentCurrency.DOScale(new Vector3(1.25f, 1.25f, 4f), flightTime * .2f))
            .Append(currentCurrency.DOScale(Vector3.one * .25f, flightTime * .82f))
            .AppendCallback(() =>
            {
                DataManager.Currency += currencyIncrease;

                currentCurrency.DOKill();
                Destroy(currentCurrency.gameObject);
            });

        currentCurrency.DOPath(new[] { midPosition, endPosition }, flightTime - .3f, PathType.CatmullRom)
            .SetEase(Ease.OutQuad)
            .SetDelay(.3f);
    }
}