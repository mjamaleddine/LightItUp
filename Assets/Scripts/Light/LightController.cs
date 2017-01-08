using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LightController : MonoBehaviour
{
    public bool IsLightEnabled { get; private set; }
    public Vector3 BeamPosition { get; private set; }
    public ShadowRayScan LightBeam;

    private SpriteRenderer _Renderer;
    private Renderer _LightBeamRenderer;

    public void Init(IEnumerable<WallController> wallControllers)
    {
        _Renderer = GetComponent<SpriteRenderer>();

        _LightBeamRenderer = LightBeam.GetComponent<MeshRenderer>();
        BeamPosition = LightBeam.transform.position;

        LightBeam.InitLightMesh(wallControllers);
    }

    private void TurnOn()
    {
        _Renderer.color = Color.black;
        _LightBeamRenderer.enabled = true;
        //_LightPoleMaterial.color = EnabledLightPoleColor;

        //_AudioSource.clip = CurrentLevel.LightOnClip;
        //_AudioSource.Play();

        IsLightEnabled = true;
    }

    private void TurnOff()
    {
        _Renderer.color = Color.white;
        _LightBeamRenderer.enabled = false;
        //_LightPoleMaterial.color = DisabledLightPoleColor;

        //_AudioSource.clip = CurrentLevel.LightOffClip;
        //_AudioSource.Play();

        IsLightEnabled = false;
    }

    internal float DistanceToMouse(Vector3 mousePosition)
    {
        return Vector2.Distance(transform.position, mousePosition);
    }

    internal void Toggle()
    {
        if (IsLightEnabled)
            TurnOff();
        else
            TurnOn();
    }
}
