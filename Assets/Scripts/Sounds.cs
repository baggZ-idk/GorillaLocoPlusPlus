using UnityEngine;

namespace GorillaLocomotion
{
    [System.Serializable]
    public class SurfaceSound
    {
        public Material[] Materials;
        public AudioClip[] Sounds;
    }

    public class Sounds : MonoBehaviour
    {
        [Header("Surface Sounds")]
        public SurfaceSound[] Surfaces;

        [Header("Default Sounds")]
        public AudioClip[] DefaultSounds;

        public AudioClip GetSound(Material material)
{
    if (material != null && Surfaces != null)
    {
        foreach (SurfaceSound surface in Surfaces)
        {
            if (surface == null || surface.Materials == null ||
                surface.Sounds == null || surface.Sounds.Length == 0)
                continue;

            foreach (Material candidate in surface.Materials)
            {
                if (candidate == null)
                    continue;

                bool matches = candidate == material ||
                    candidate.name == material.name;

                if (matches)
                {
                    return surface.Sounds[
                        UnityEngine.Random.Range(0, surface.Sounds.Length)
                    ];
                }
            }
        }
    }

    if (DefaultSounds != null && DefaultSounds.Length > 0)
    {
        return DefaultSounds[
            UnityEngine.Random.Range(0, DefaultSounds.Length)
        ];
    }

    return null;
}
    }
}
