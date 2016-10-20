﻿using UnityEngine;
using System.Collections;
using System;
using System.ComponentModel;
using UnityEngine.UI;

public class Annotation : MonoBehaviour {

    [DefaultValue("")]
	public string creator;
	public DateTime creationDate;
	//needs to be public so clone dont "loose" knowledge of his label ... but still child
	public GameObject myAnnotationLabel;
	public GameObject myAnnotationListEntry;
	private Color myColor;
	private Color defaultColor;
	public Material defaultMaterial, previewMaterial;

    // Use this for initialization
    void Start () {
        creationDate = DateTime.Now;
		defaultColor = defaultMaterial.color;
		defaultMaterial = Instantiate (defaultMaterial);
		previewMaterial = Instantiate (previewMaterial);
		makeOpaque ();
    }

    // Update is called once per frame
	void Update () {
        //Update lines from annotation point to label
        //because lines are no game objects

        if (myAnnotationLabel != null)
        {
			this.GetComponent<LineRenderer> ().enabled = true;
			this.GetComponent<LineRenderer>().SetPosition(0, this.transform.position);
            this.GetComponent<LineRenderer>().SetPosition(1, this.myAnnotationLabel.transform.position);
		} else {
			this.GetComponent<LineRenderer> ().enabled = false;
		}
        
    }


	//Used to create new Label
	public void CreateLabel(GameObject annotationLabelMaster) {
		//Create Label
		myAnnotationLabel = (GameObject)Instantiate(annotationLabelMaster, new Vector3(0f,0f,15f), this.transform.localRotation);
		myAnnotationLabel.transform.localScale = new Vector3 (0.05f, 0.05f, 0.05f);	//*= meshNode.transform.localScale.x; //x,y,z are the same
		myAnnotationLabel.transform.SetParent(this.transform, false);
		myAnnotationLabel.SetActive (true);
		myAnnotationLabel.GetComponent<AnnotationLabel> ().setLabelText(annotationLabelMaster.GetComponent<AnnotationLabel>().getLabelText());
		//Create line form point to label
		this.GetComponent<LineRenderer>().SetPosition(0, this.transform.position);
		this.GetComponent<LineRenderer>().SetPosition(1, this.myAnnotationLabel.transform.position);
	}

	//Destroys Annotation and Label
	public void destroyAnnotation() {
		destroyLabel ();
		Destroy (this.gameObject);
	}

	public void destroyLabel() {
		Destroy (myAnnotationLabel);
		myAnnotationLabel = null;
	}

	//Updates the Label Text
	public void setLabeText(String newLabel) {

		if(myAnnotationLabel == null) {
			Debug.LogError("Annotation has no AnnotationLabel to edit");
			return;
		}
		// Change label text:
		myAnnotationLabel.GetComponent<AnnotationLabel> ().setLabelText (newLabel);
	}



	public void updatePosition(Quaternion rotation, Vector3 position) {
		this.GetComponent<Transform> ().localPosition = position;
		this.GetComponent<Transform> ().localRotation = rotation;
	}
		
	//to get AnnotationLabel
	public GameObject getLabel() {
		return myAnnotationLabel;
	}

	//to get Annotation Label text
	public String getLabelText() {
		return myAnnotationLabel.GetComponent<AnnotationLabel> ().getLabelText();
	}

	//to get Color
	public Color getColor() {
		return new Color(myColor.r, myColor.g, myColor.b, this.GetComponent<Renderer> ().material.color.a);
	}

	//Used to save Label Changes
	public void saveLabelChanges() {
		if(myAnnotationListEntry != null) {
			myAnnotationListEntry.GetComponent<AnnotationListEntry>().updateLabel (getLabelText ());
		}
	}

	//used to change color of Annotation
	public void changeColor(Color newColor) {
		myColor = new Color(newColor.r, newColor.g, newColor.b, this.GetComponent<Renderer> ().material.color.a);
		this.gameObject.GetComponent<Renderer> ().material.color = new Color(newColor.r, newColor.g, newColor.b, this.GetComponent<Renderer> ().material.color.a);
	}

	//used to change color of Annotation
	public void setDefaultColor() {
		myColor = new Color(defaultColor.r, defaultColor.g, defaultColor.b, defaultColor.a);
		this.GetComponent<Renderer> ().material.color = new Color(defaultColor.r, defaultColor.g, defaultColor.b, this.GetComponent<Renderer> ().material.color.a);
	}

	public void makeTransperent() {
		this.GetComponent<Renderer> ().material = previewMaterial;
		this.GetComponent<Renderer> ().material.color = new Color(myColor.r, myColor.g, myColor.b, 0.2f);
	}

	public void makeOpaque() {
		this.GetComponent<Renderer> ().material = defaultMaterial;
		this.GetComponent<Renderer> ().material.color = new Color(myColor.r, myColor.g, myColor.b, 1.0f);
	}

	public void disableCollider() {
			this.GetComponent<SphereCollider> ().enabled = false;
	}
}
