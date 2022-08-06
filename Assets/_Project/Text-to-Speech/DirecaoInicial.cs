using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ENA.TTS
{
	public class DirecaoInicial: MonoBehaviour
	{
		public static int ajudaInicial;
		public Transform pontoInicial;
		// direções por vetor
		// 0 = frente
		// 1 = atras
		// 2 = direita
		// 3 = esquerda
		// 4 = frente e direita
		// 5 = frente e esquerda
		// 6 = atras e direita
		// 7 = atras e esquerda
		public bool aproximadamenteX,aproximadamenteZ;

		public bool frente,atras,esquerda,direita;
		void Start () {
			ajudaInicial = 0;
		}
		
		void Update () {
			transform.position = pontoInicial.position;
			CalculoAproximado();
			Calcular();
			if(UnityEngine.Input.GetButtonDown("Fire1")){
				ajudaInicial++;
				CalculoDirecao();
			}
			
		}
		
		void Calcular(){
			if(transform.localPosition.x < 0.5f && !aproximadamenteX){
				esquerda = true;
			}else{
				esquerda = false;
			}
			if(transform.localPosition.x > 0.5f && !aproximadamenteX){
				direita = true;
			}else{
				direita = false;
			}
			if(transform.localPosition.z > 0.5f && !aproximadamenteZ){
				frente = true;
			}else{
				frente = false;
			}
			if(transform.localPosition.z < 0.5f && !aproximadamenteZ){
				atras = true;
			}else{
				atras = false;
			}
			
		}

		void CalculoAproximado(){
			if(transform.localPosition.x > -0.5f && transform.localPosition.x < 0.5f){
				aproximadamenteX = true;
			}else
			{
				aproximadamenteX = false;
			}

			if(transform.localPosition.z > -0.5f && transform.localPosition.z < 0.5f){
				aproximadamenteZ = true;
			}else
			{
				aproximadamenteZ = false;
			}
		}
		void CalculoDirecao(){
			if(frente && !direita && !esquerda){
				print("frente");
				Falar(0);
			}else if(atras && !direita && !esquerda){
				print("atras");
				Falar(1);
			}else if(direita && !frente && !atras){
				print("direita");
				Falar(2);
			}else if(esquerda && !frente && !atras){
				print("esquerda");
				Falar(3);
			}else if(frente && direita){
				print("frente e direita");
				Falar(4);
			}else if(frente && esquerda){
				print("frente e esquerda");
				Falar(5);
			}else if(atras && direita){
				print("atras e direita");
				Falar(6);
			}else if(atras && esquerda){
				print("atras e esquerda");
				Falar(7);
			}
		}

		void Falar(int direcao){
			// if(Tradutor2.portugues){
			// 	UAP_AccessibilityManager.Say("O ponto inicial está " + portugues[direcao], false);
			// 	print("O ponto inicial está " + portugues[direcao]);
			// }else if(Tradutor2.ingles){
			// 	UAP_AccessibilityManager.Say("The starting point is "+ingles[direcao], false);
			// }else if(Tradutor2.espanhol){
			// 	UAP_AccessibilityManager.Say("El punto inicial está "+espanhol[direcao], false);
			// }
		}
	}
}
