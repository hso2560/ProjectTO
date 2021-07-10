using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class LobbyManager : MonoBehaviour
{
    public TextMeshProUGUI titleTxt;
    public Text bestTxt;
    [SerializeField] Text testText;
    public GameObject soundPref;

    [SerializeField] private bool isTestMode;

    private void Awake()
    {
        testText.gameObject.SetActive(isTestMode);
        Cursor.lockState = CursorLockMode.None;
        PoolManager.CreatePool<SoundPrefab>(soundPref, GameManager.Instance.soundPoolParent, 6);
    }
    private void Start()
    {
        titleTxt.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 2).SetLoops(-1, LoopType.Yoyo);
        titleTxt.DOFade(0.65f, 5).SetLoops(-1, LoopType.Yoyo);
        
    }

    public void SetBestScore(bool isClear, float bestTime)
    {
        if (GameManager.Instance.savedData.option.language == Language.Korean)
        {
            if (isClear)
                bestTxt.text = string.Format("�ְ���: {0}�ð� {1}�� {2}��", (int)bestTime / 3600, ((int)bestTime / 60) % 60, (int)bestTime % 60);
            else
                bestTxt.text = "�ְ���: Ŭ���� ��� ����";
        }
        else if(GameManager.Instance.savedData.option.language == Language.English)
        {
            if (isClear)
                bestTxt.text = string.Format("Highest Record: {0}h {1}m {2}s", (int)bestTime / 3600, ((int)bestTime / 60) % 60, (int)bestTime % 60);
            else
                bestTxt.text = "Highest Record: There is no clear record.";
        }
    }

    #region �ּ�
    /*private void Update()
    {
        titleTxt.ForceMeshUpdate();
        TMP_TextInfo textInfo = titleTxt.textInfo;

        for(int i=0; i<textInfo.characterCount; ++i)
        {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];  //var�� �����͵� ��

            if(!charInfo.isVisible)
            {
                continue;
            }

            Vector3[] verts = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;

            for(int j=0; j<4; ++j)
            {
                Vector3 orig = verts[charInfo.vertexIndex+j];
                verts[charInfo.vertexIndex + j] = orig + new Vector3(0, Mathf.Sin(Time.time * 2f + orig.x * 0.01f) * 10f, 0);
            }
        }

        for(int i=0; i<textInfo.meshInfo.Length; ++i)
        {
            TMP_MeshInfo meshInfo = textInfo.meshInfo[i];
            meshInfo.mesh.vertices = meshInfo.vertices;
            titleTxt.UpdateGeometry(meshInfo.mesh, i);
        }
    }*/
    #endregion
}
