using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;

public class TableScript : MonoBehaviour {

	public float d_const = 0.19f;
	public float h_const = 0.45f;
	public float m_const = 0.2f;
	public float l_const = 0.02f;
	public float bigd_const = 0.04f;
	public float r_const = 0.021f;
	public float eps_const = 0.84f;
	public float h_cons = 0.0005f;
	public float r_cons = 0.0001f;

	[SerializeField]
    InputField textInput;
	[SerializeField]
    Text t1;
    [SerializeField]
    Text t2;
    [SerializeField]
    Text t3;
    [SerializeField]
    Text t_sht;
    [SerializeField]
    Text delta_t;
    [SerializeField]
    Text eps_sht;
    [SerializeField]
    Text eps_t;
    [SerializeField]
    Text delta_h;
	[SerializeField]
    Text delta_r;
	[SerializeField]
    Text delta_eps;
	[SerializeField]
    Text sigma;
	[SerializeField]
    Text iz;
	[SerializeField]
    Text eps_r;


	public void OnButtonClicked()
    {
        if (t1.text == "-")
        {
            t1.text = textInput.text.ToString();
        }
        else if (t2.text == "-")
        {
            t2.text = textInput.text.ToString();
        }
        else if (t3.text == "-")
        {
            t3.text = textInput.text.ToString();
        }
        else 
        {
            float t_sr = (float.Parse(t1.text)+float.Parse(t2.text)+float.Parse(t3.text)/3);
			t_sht.text = t_sr.ToString();
            float eps1 = (2 * h_const) / (r_const * (float)Mathf.Pow(t_sr, 2));
			eps_sht.text = eps1.ToString();
            float s = Mathf.Sqrt((1f/3f)*(Mathf.Pow((float.Parse(t1.text) - t_sr),2)+(float)Mathf.Pow((float.Parse(t2.text) - t_sr),2)+(float)Mathf.Pow((float.Parse(t3.text) - t_sr),2)));
            float del_t = (2.6f * s/Mathf.Sqrt(3f));
			delta_t.text = del_t.ToString();
			eps_t.text = eps_const.ToString();
			delta_h.text = h_cons.ToString();
			delta_r.text = r_cons.ToString();
			float sig = Mathf.Sqrt(
                Mathf.Pow(h_cons / h_const, 2) +
                Mathf.Pow(r_cons / r_const, 2) +
                4 * Mathf.Pow(del_t / t_sr, 2)
            ) * 100;
			sigma.text = sig.ToString();
			float del_eps = (sig * (eps_const/2));
			delta_eps.text = del_eps.ToString();
			float i1 = (m_const * (float)Mathf.Pow(d_const,2)); 
			float izet = (0.00471f + (4 * i1));
			iz.text = izet.ToString();
            float ep = (m_const * 10 * r_const);
			float ept = (izet + (m_const * (float)Mathf.Pow( r_const, 2)));
			float itog = (ep/ept);
			eps_r.text = itog.ToString();
        }
        textInput.text = "";
    }

	public void CleanNewVariables()
    {
        textInput.text = ""; 
        ResetTextToDefault(t1);
        ResetTextToDefault(t2);
        ResetTextToDefault(t3);
        ResetTextToDefault(t_sht);
        ResetTextToDefault(delta_t);
        ResetTextToDefault(eps_sht);
        ResetTextToDefault(eps_t);
        ResetTextToDefault(delta_h);
        ResetTextToDefault(delta_r);
        ResetTextToDefault(delta_eps);
        ResetTextToDefault(sigma);
        ResetTextToDefault(iz);
        ResetTextToDefault(eps_t);
    }

    void ResetTextToDefault(Text textComponent)
    {
        if (textComponent != null)
        {
            textComponent.text = "-";
        }
    }
}
