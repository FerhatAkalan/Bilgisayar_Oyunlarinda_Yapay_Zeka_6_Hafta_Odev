using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain_sc : MonoBehaviour
{
	ANN_sc ann;// Yapay sinir ağı nesnesi
	double sumSquareError = 0;// Hata ölçümü için kullanılan toplam kare hata (SSE)

	void Start () {
		// Yapay sinir ağı parametreleri
		int numberOfInputs = 2;// Giriş nöronlarının sayısı
		int numberOfOutputs = 1;// Çıkış nöronlarının sayısı
		int numberOfHiddenLayers = 1;// Gizli katman sayısı
		int numberOfNeuronsPerHiddenLayer = 2;// Her gizli katmandaki nöron sayısı
		double alpha = 0.8;// Öğrenme oranı

		int epoch = 1000;// Eğitim döngü sayısı (epoch)

		ann = new ANN_sc(numberOfInputs, numberOfOutputs, 
						numberOfHiddenLayers, 
						numberOfNeuronsPerHiddenLayer, 
						alpha);
		// Çıkış sonuçlarını saklamak için kullanılan liste
		List<double> result;
		// Eğitim döngüsü
		for(int i = 0; i < epoch; i++)
		{
			sumSquareError = 0;
			result = Train(1, 1, 0);
			sumSquareError += Mathf.Pow((float)result[0] - 0, 2);
			result = Train(1, 0, 1);
			sumSquareError += Mathf.Pow((float)result[0] - 1, 2);
			result = Train(0, 1, 1);
			sumSquareError += Mathf.Pow((float)result[0] - 1, 2);
			result = Train(0, 0, 0);
			sumSquareError += Mathf.Pow((float)result[0] - 0, 2);
		}
		// Eğitim tamamlandıktan sonra toplam kare hatayı yazdırıyoruz
		Debug.Log("SSE: " + sumSquareError);
		// Eğitim sonrası modeli test ediyoruz ve sonuçları yazdırıyoruz
		result = Train(1, 1, 0);
		Debug.Log(" 1 1 " + result[0]);
		result = Train(1, 0, 1);
		Debug.Log(" 1 0 " + result[0]);
		result = Train(0, 1, 1);
		Debug.Log(" 0 1 " + result[0]);
		result = Train(0, 0, 0);
		Debug.Log(" 0 0 " + result[0]);
	}
	// Eğitim işlemi, giriş-çıkış çiftlerini alır ve ağı çalıştırır
	List<double> Train(double input1, double input2, double output)
	{
		List<double> inputs = new List<double>(){input1, input2};
		List<double> outputs = new List<double>(){output};
		return (ann.Run(inputs, outputs));
	}
}