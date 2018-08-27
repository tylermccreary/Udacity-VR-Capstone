using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class VoiceRecognition : MonoBehaviour {
    public GameManager gameManager;

    [SerializeField]
    private string[] m_Keywords;

    private KeywordRecognizer m_Recognizer;

    void Start()
    {
        m_Keywords = new string[2];
        m_Keywords[0] = "open";
        m_Keywords[1] = "shoot";

        m_Recognizer = new KeywordRecognizer(m_Keywords);
        m_Recognizer.OnPhraseRecognized += OnPhraseRecognized;
        m_Recognizer.Start();
    }

    private void OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        Debug.Log(args.text);
        gameManager.RecognizeSpeech(args.text);
    }
}
