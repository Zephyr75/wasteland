using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum Cutscene
{
    MINE, INFILTRATION, BALLOON
}

public class CutsceneSystem : MonoBehaviour
{
    public Cutscene cut;
    private List<string> dialoguePlayer = new List<string>();
    private List<string> dialogueThomas = new List<string>();
    private List<string> dialogueEsteban = new List<string>();
    public GameObject displayPlayer, displayThomas, displayEsteban, cutsceneCam, player;
    private string current;
    private List<(string, int)> waiting = new List<(string, int)>();
    private bool inDialogue;
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        dialoguePlayer.Add("Hey, j'ai vu que vous aviez besoin d'aide par ici, je peux aider?");
        dialoguePlayer.Add("Je manque encore d'entrainement mais si ça pose pas de problèmes, je suis chaud!");
        dialoguePlayer.Add("Laisse moi réussir une mission pour voir si mes créas valent mieux que des brouillons!");
        dialoguePlayer.Add("Ca me semble clair, espérons que ça se passe bien!");
        dialoguePlayer.Add("Merci, je vous fais confiance haha, t'inquiètes.");
        
        dialogueThomas.Add("Ouais, on cherche un volontaire pour aller voler des moteurs pour hélices, on a plus rien..." + "/" +
            "Ca t'intéresse?");
        dialogueThomas.Add("T'inquiètes pas pour ça haha! Tu vas faire le trajet en véhicule et il parait que t'es notre expert dans ce domaine là!");
        dialogueThomas.Add("Je me fais pas de souçi pour ça t'inquiètes. Je vais t'expliquer rapidement en quoi ça consiste:" + "/" +
            "L'objectif est en ligne droite devant nous, tu vas traverser la rivière et passer rapidement à travers leur base sans qu'ils s'y attendent." + "/" +
            "Essaye de créer un véhicule assez léger et rapide, ça devrait faire l'affaire." + "/" +
            "Une fois dans leur base, on t'expliquera où trouver les moteurs, ils devraient pas être trop protégés." + "/" +
            "On viendra te récupérer avec une corde depuis notre dernier hélico");
        dialogueThomas.Add("Bon courage frérot et t'inquiètes pas pour la récupération, on gère!");
    }

    // Update is called once per frame
    void Update()
    {
        if (waiting.Count > 0 && !inDialogue)
        {
            PlayDialogue(waiting[0].Item1, waiting[0].Item2);
            inDialogue = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //Player => 0
            //Thomas => 1
            //Esteban => 2
            if (cut == Cutscene.INFILTRATION)
            {
                StartCoroutine(ActivateAnim(0, "Infiltration"));
                StartCoroutine(DeactivateCam(30, "Infiltration"));
            }
            if (cut == Cutscene.MINE)
            {
                waiting.Add((dialoguePlayer[0], 0));
                waiting.Add((dialogueThomas[0], 1));
                waiting.Add((dialoguePlayer[1], 0));
                waiting.Add((dialogueThomas[1], 1));
                waiting.Add((dialoguePlayer[2], 0));
                waiting.Add((dialogueThomas[2], 1));
                waiting.Add((dialoguePlayer[3], 0));
                waiting.Add((dialogueThomas[3], 1));
                waiting.Add((dialoguePlayer[4], 0));
                StartCoroutine(ActivateAnim(24, "Mine"));
                StartCoroutine(DeactivateCam(49, "Mine"));
            }
        }
    }

    private void PlayDialogue(string dialogue, int talking)
    {
        switch (talking)
        {
            case 0: displayPlayer.SetActive(true); break;
            case 1: displayThomas.SetActive(true); break;
            default: displayEsteban.SetActive(true); break;
        }
        float time = 0;
        current = "";
        for (int i = 0; i < dialogue.Length; i++)
        {
            StartCoroutine(AddLetter(time, dialogue.Substring(i, 1)));
            if (i + 1 < dialogue.Length)
            {
                if (dialogue.Substring(i + 1, 1) == "/")
                {
                    time += 1.03f;
                }
                else
                {
                    time += .03f;
                }
            }
            else
            {
                time += .03f;
            }
            
        }
        StartCoroutine(CloseDialogue(time + 1));
    }

    IEnumerator AddLetter(float time, string letter)
    {
        yield return new WaitForSeconds(time);
        if (letter == "/")
        {
            current = "";
        }
        else
        {
            current += letter;
        }
        displayPlayer.GetComponentInChildren<TMP_Text>().text = current;
        displayThomas.GetComponentInChildren<TMP_Text>().text = current;
        displayEsteban.GetComponentInChildren<TMP_Text>().text = current;
    }

    IEnumerator CloseDialogue(float time)
    {
        yield return new WaitForSeconds(time);
        waiting.RemoveAt(0);
        displayPlayer.SetActive(false);
        displayThomas.SetActive(false);
        displayEsteban.SetActive(false);
        inDialogue = false;
    }

    IEnumerator DeactivateCam(float time, string animation)
    {
        yield return new WaitForSeconds(time);
        player.GetComponentInChildren<Camera>().enabled = true;
        cutsceneCam.SetActive(false);
        anim.SetBool(animation, false);
        Destroy(gameObject);
    }

    IEnumerator ActivateAnim(float time, string animation)
    {
        yield return new WaitForSeconds(time);
        cutsceneCam.SetActive(true);
        player.GetComponentInChildren<Camera>().enabled = false;
        anim.SetBool(animation, true);
    }



}
