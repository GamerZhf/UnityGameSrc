using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class UnityDataConnector : MonoBehaviour
{
    private string currentStatus;
    public GameObject dataDestinationObject;
    public bool debugMode;
    private Rect guiBoxRect;
    private Rect guiButtonRect;
    private Rect guiButtonRect2;
    private Rect guiButtonRect3;
    public float maxWaitTime = 10f;
    public string password = string.Empty;
    private bool saveToGS;
    public string spreadsheetId = string.Empty;
    private JsonData[] ssObjects;
    public string statisticsWorksheetName = "Statistics";
    private bool updating;
    public string webServiceUrl = string.Empty;
    public string worksheetName = string.Empty;

    private void Connect()
    {
        if (!this.updating)
        {
            this.updating = true;
            base.StartCoroutine(this.GetData());
        }
    }

    [DebuggerHidden]
    private IEnumerator GetData()
    {
        <GetData>c__Iterator1A iteratora = new <GetData>c__Iterator1A();
        iteratora.<>f__this = this;
        return iteratora;
    }

    private void OnGUI()
    {
        GUI.Box(this.guiBoxRect, this.currentStatus);
        if (GUI.Button(this.guiButtonRect, "Update From Google Spreadsheet"))
        {
            this.Connect();
        }
        this.saveToGS = GUI.Toggle(this.guiButtonRect2, this.saveToGS, "Save Stats To Google Spreadsheet");
        if (GUI.Button(this.guiButtonRect3, "Reset Balls values"))
        {
            this.dataDestinationObject.SendMessage("ResetBalls");
        }
    }

    public void SaveDataOnTheCloud(string ballName, float collisionMagnitude)
    {
        if (this.saveToGS)
        {
            base.StartCoroutine(this.SendData(ballName, collisionMagnitude));
        }
    }

    [DebuggerHidden]
    private IEnumerator SendData(string ballName, float collisionMagnitude)
    {
        <SendData>c__Iterator1B iteratorb = new <SendData>c__Iterator1B();
        iteratorb.ballName = ballName;
        iteratorb.collisionMagnitude = collisionMagnitude;
        iteratorb.<$>ballName = ballName;
        iteratorb.<$>collisionMagnitude = collisionMagnitude;
        iteratorb.<>f__this = this;
        return iteratorb;
    }

    private void Start()
    {
        this.updating = false;
        this.currentStatus = "Offline";
        this.saveToGS = false;
        this.guiBoxRect = new Rect(10f, 10f, 310f, 140f);
        this.guiButtonRect = new Rect(30f, 40f, 270f, 30f);
        this.guiButtonRect2 = new Rect(30f, 75f, 270f, 30f);
        this.guiButtonRect3 = new Rect(30f, 110f, 270f, 30f);
    }

    [CompilerGenerated]
    private sealed class <GetData>c__Iterator1A : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal UnityDataConnector <>f__this;
        internal string <connectionString>__0;
        internal float <elapsedTime>__2;
        internal string <response>__3;
        internal WWW <www>__1;

        [DebuggerHidden]
        public void Dispose()
        {
            this.$PC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 0:
                {
                    string[] textArray1 = new string[] { this.<>f__this.webServiceUrl, "?ssid=", this.<>f__this.spreadsheetId, "&sheet=", this.<>f__this.worksheetName, "&pass=", this.<>f__this.password, "&action=GetData" };
                    this.<connectionString>__0 = string.Concat(textArray1);
                    if (this.<>f__this.debugMode)
                    {
                        UnityEngine.Debug.Log("Connecting to webservice on " + this.<connectionString>__0);
                    }
                    this.<www>__1 = new WWW(this.<connectionString>__0);
                    this.<elapsedTime>__2 = 0f;
                    this.<>f__this.currentStatus = "Stablishing Connection... ";
                    break;
                }
                case 1:
                    break;

                default:
                    goto Label_02EE;
            }
            while (!this.<www>__1.isDone)
            {
                this.<elapsedTime>__2 += Time.deltaTime;
                if (this.<elapsedTime>__2 >= this.<>f__this.maxWaitTime)
                {
                    this.<>f__this.currentStatus = "Max wait time reached, connection aborted.";
                    UnityEngine.Debug.Log(this.<>f__this.currentStatus);
                    this.<>f__this.updating = false;
                    break;
                }
                this.$current = null;
                this.$PC = 1;
                return true;
            }
            if (!this.<www>__1.isDone || !string.IsNullOrEmpty(this.<www>__1.error))
            {
                this.<>f__this.currentStatus = "Connection error after" + this.<elapsedTime>__2.ToString() + "seconds: " + this.<www>__1.error;
                UnityEngine.Debug.LogError(this.<>f__this.currentStatus);
                this.<>f__this.updating = false;
            }
            else
            {
                this.<response>__3 = this.<www>__1.text;
                UnityEngine.Debug.Log(this.<elapsedTime>__2 + " : " + this.<response>__3);
                this.<>f__this.currentStatus = "Connection stablished, parsing data...";
                if (this.<response>__3 == "\"Incorrect Password.\"")
                {
                    this.<>f__this.currentStatus = "Connection error: Incorrect Password.";
                    UnityEngine.Debug.LogError(this.<>f__this.currentStatus);
                    this.<>f__this.updating = false;
                }
                else
                {
                    try
                    {
                        this.<>f__this.ssObjects = JsonMapper.ToObject<JsonData[]>(this.<response>__3);
                    }
                    catch
                    {
                        this.<>f__this.currentStatus = "Data error: could not parse retrieved data as json.";
                        UnityEngine.Debug.LogError(this.<>f__this.currentStatus);
                        this.<>f__this.updating = false;
                        goto Label_02EE;
                    }
                    this.<>f__this.currentStatus = "Data Successfully Retrieved!";
                    this.<>f__this.updating = false;
                    this.<>f__this.dataDestinationObject.SendMessage("DoSomethingWithTheData", this.<>f__this.ssObjects);
                    this.$PC = -1;
                }
            }
        Label_02EE:
            return false;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        object IEnumerator<object>.Current
        {
            [DebuggerHidden]
            get
            {
                return this.$current;
            }
        }

        object IEnumerator.Current
        {
            [DebuggerHidden]
            get
            {
                return this.$current;
            }
        }
    }

    [CompilerGenerated]
    private sealed class <SendData>c__Iterator1B : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal string <$>ballName;
        internal float <$>collisionMagnitude;
        internal UnityDataConnector <>f__this;
        internal string <connectionString>__0;
        internal float <elapsedTime>__2;
        internal string <response>__3;
        internal WWW <www>__1;
        internal string ballName;
        internal float collisionMagnitude;

        [DebuggerHidden]
        public void Dispose()
        {
            this.$PC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 0:
                    if (this.<>f__this.saveToGS)
                    {
                        string[] textArray1 = new string[] { this.<>f__this.webServiceUrl, "?ssid=", this.<>f__this.spreadsheetId, "&sheet=", this.<>f__this.statisticsWorksheetName, "&pass=", this.<>f__this.password, "&val1=", this.ballName, "&val2=", this.collisionMagnitude.ToString(), "&action=SetData" };
                        this.<connectionString>__0 = string.Concat(textArray1);
                        if (this.<>f__this.debugMode)
                        {
                            UnityEngine.Debug.Log("Connection String: " + this.<connectionString>__0);
                        }
                        this.<www>__1 = new WWW(this.<connectionString>__0);
                        this.<elapsedTime>__2 = 0f;
                        break;
                    }
                    goto Label_01D6;

                case 1:
                    break;

                default:
                    goto Label_01D6;
            }
            while (!this.<www>__1.isDone)
            {
                this.<elapsedTime>__2 += Time.deltaTime;
                if (this.<elapsedTime>__2 >= this.<>f__this.maxWaitTime)
                {
                    break;
                }
                this.$current = null;
                this.$PC = 1;
                return true;
            }
            if (this.<www>__1.isDone && string.IsNullOrEmpty(this.<www>__1.error))
            {
                this.<response>__3 = this.<www>__1.text;
                if (!this.<response>__3.Contains("Incorrect Password") && !this.<response>__3.Contains("RCVD OK"))
                {
                    this.$PC = -1;
                }
            }
        Label_01D6:
            return false;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        object IEnumerator<object>.Current
        {
            [DebuggerHidden]
            get
            {
                return this.$current;
            }
        }

        object IEnumerator.Current
        {
            [DebuggerHidden]
            get
            {
                return this.$current;
            }
        }
    }
}

