using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FontReflecter_Nanamune : MonoBehaviour
{
    //SetFontÇ≈égÇ§ïœêî
    [SerializeField] private List<TextMeshProUGUI> tMP_MessageTexts;
    [SerializeField] private List<TMP_FontAsset> newTMPFontAssets = new List<TMP_FontAsset>();
    private void Start()
    {
        switch (PublicStaticStatus.Font)
        {
            case 0:
                foreach(TextMeshProUGUI tMP in tMP_MessageTexts)
                {
                    tMP.font = newTMPFontAssets[0];
                }
                break;
            case 1:
                foreach (TextMeshProUGUI tMP in tMP_MessageTexts)
                {
                    tMP.font = newTMPFontAssets[1];
                }
                break;
            case 2:
                foreach (TextMeshProUGUI tMP in tMP_MessageTexts)
                {
                    tMP.font = newTMPFontAssets[2];
                }
                break;
        }
    }
}
