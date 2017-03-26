using UnityEngine;
using System.Collections;

public class Singleton<Instance> : MonoBehaviour where Instance : Singleton<Instance> {
	
	private static Instance instance;
	public bool dontDestroyOnLoad;
	
	public virtual void OnEnable() {

		if(dontDestroyOnLoad) {
			if(!instance) {
				instance = this as Instance;
			}
			else {
				DestroyObject(gameObject);
			}
			DontDestroyOnLoad(gameObject);
		}
		else {
			instance = this as Instance;
		}
	}
}