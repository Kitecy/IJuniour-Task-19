using TMPro;
using UnityEngine;

public class BaseView : MonoBehaviour
{
    [SerializeField] private Base _base;
    [SerializeField] private TMP_Text _resourcesText;

    private void OnEnable()
    {
        _base.ResourcesChanged += OnResourcesChanged;
    }

    private void OnDisable()
    {
        _base.ResourcesChanged -= OnResourcesChanged;
    }

    private void OnResourcesChanged(int resources)
    {
        _resourcesText.text = resources.ToString();
    }
}
