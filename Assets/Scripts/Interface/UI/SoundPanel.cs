using UnityEngine;
using UnityEngine.UI;
using FMODUnity;
using FMOD.Studio;

public class SoundPanel : MonoBehaviour, IMenuPanel
{
    [Header("UI Sliders")]
    //[SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    //[SerializeField] private Slider sfxSlider;
    //[SerializeField] private Slider videoSlider;

    [Header("FMOD Buses")]
    private Bus masterBus;
    private Bus musicBus;
    private Bus sfxBus;
    private Bus videoBus;

    private void Awake()
    {
        //masterBus = RuntimeManager.GetBus("bus:/Master");
        musicBus = RuntimeManager.GetBus("bus:/Master/Music");
        //sfxBus = RuntimeManager.GetBus("bus:/Master/SFX");
        //videoBus = RuntimeManager.GetBus("bus:/Master/Video");

        musicSlider.onValueChanged.AddListener(val => Debug.Log("onValueChanged! " + val));

    }

    private void Start()
    {
        // Cargar valores sin eventos
        //masterSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("MasterBus", 1f));
        musicSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("MusicBus", 1f));
        //sfxSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("SFXBus", 1f));
        //videoSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("VideoBus", 1f));

        // Aplicar volumen al iniciar
        //ApplyBusVolume(masterBus, masterSlider.value);
        ApplyBusVolume(musicBus, musicSlider.value);
        //ApplyBusVolume(sfxBus, sfxSlider.value);
        //ApplyBusVolume(videoBus, videoSlider.value);

        // Listeners
        //masterSlider.onValueChanged.AddListener(v => OnSlider(masterBus, "MasterBus", v));
        musicSlider.onValueChanged.AddListener(v => OnSlider(musicBus, "MusicBus", v));
        //sfxSlider.onValueChanged.AddListener(v => OnSlider(sfxBus, "SFXBus", v));
        //videoSlider.onValueChanged.AddListener(v => OnSlider(videoBus, "VideoBus", v));
    }

    private void OnSlider(Bus bus, string key, float value)
    {
        ApplyBusVolume(bus, value);
        PlayerPrefs.SetFloat(key, value);
    }

    private void ApplyBusVolume(Bus bus, float value)
    {
        if (bus.isValid())
            bus.setVolume(value);
    }

    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
        PlayerPrefs.Save();
    }

    private void OnDisable()
    {
        PlayerPrefs.Save();
    }
}
