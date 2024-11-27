using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ANN_sc
{
	public int numberOfInputs;// Sinir ağına giren veri sayısı
	public int numberOfOutputs;// Sinir ağının ürettiği çıktı sayısı
	public int numberOfHiddenLayers;// Gizli katman sayısı
	public int numberOfNeuronsPerHiddenLayer;// Her gizli katmanda bulunan nöron sayısı
	public double alpha;// Öğrenme oranı
	List<Layer_sc> layers = new List<Layer_sc>();// Sinir ağı katmanlarını saklayan liste
 	// Yapay sinir ağı sınıfı, verilen parametrelere göre katmanları başlatır
	public ANN_sc(int numberOfInputs, int numberOfOutputs, 
				int numberOfHiddenLayers, 
				int numberOfNeuronsPerHiddenLayer, 
				double alpha)
	{
		this.numberOfInputs = numberOfInputs;
		this.numberOfOutputs = numberOfOutputs;
		this.numberOfHiddenLayers = numberOfHiddenLayers;
		this.numberOfNeuronsPerHiddenLayer = numberOfNeuronsPerHiddenLayer;
		this.alpha = alpha;
		// Gizli katmanlar varsa başlatılır
		if(numberOfHiddenLayers > 0)
		{
			layers.Add(new Layer_sc(numberOfNeuronsPerHiddenLayer, numberOfInputs));

			for(int i = 0; i < numberOfHiddenLayers-1; i++)
			{
				layers.Add(new Layer_sc(numberOfNeuronsPerHiddenLayer, numberOfNeuronsPerHiddenLayer));
			}

			layers.Add(new Layer_sc(numberOfOutputs, numberOfNeuronsPerHiddenLayer));
		}
		else
		{
			layers.Add(new Layer_sc(numberOfOutputs, numberOfInputs));
		}
	}
	// Sinir ağını çalıştırır, verilen giriş değerleriyle sonuçları üretir
	public List<double> Run(List<double> inputValues, List<double> desiredOutput)
	{
		List<double> inputs = new List<double>();
		List<double> outputs = new List<double>();
		// Giriş veri sayısının kontrolü
		if(inputValues.Count != numberOfInputs)
		{
			Debug.Log("ERROR: Number of Inputs must be " + numberOfInputs);
			return outputs;
		}

		inputs = new List<double>(inputValues);
		// Tüm katmanlarda ileri yayılım işlemi yapılır
		for(int i = 0; i < numberOfHiddenLayers + 1; i++)
		{
				if(i > 0)// İlk katmandan sonraki katmanlar, bir önceki katmandan çıktı alır
				{
					inputs = new List<double>(outputs);
				}
				outputs.Clear();

				for(int j = 0; j < layers[i].numberOfNeurons; j++)
				{
					double N = 0;
					layers[i].neurons[j].inputs.Clear();// Nöron girişleri temizlenir
 					// Her bir girdinin ağırlığı ile çarpımı hesaplanır
					for(int k = 0; k < layers[i].neurons[j].numberOfInputs; k++)
					{
					    layers[i].neurons[j].inputs.Add(inputs[k]);
						N += layers[i].neurons[j].weights[k] * inputs[k];
					}

					N -= layers[i].neurons[j].bias;
					layers[i].neurons[j].output = ActivationFunction(N);
					outputs.Add(layers[i].neurons[j].output);
				}
		}
		// Eğitim sırasında ağırlıkları güncelle
		UpdateWeights(outputs, desiredOutput);

		return outputs;
	}
	// Ağırlıkları günceller, geri yayılım algoritmasıyla hatayı minimize eder	
	void UpdateWeights(List<double> outputs, List<double> desiredOutput)
	{
		double error;
		for(int i = numberOfHiddenLayers; i >= 0; i--)
		{
			for(int j = 0; j < layers[i].numberOfNeurons; j++)
			{
				if(i == numberOfHiddenLayers)
				{
					error = desiredOutput[j] - outputs[j];
					layers[i].neurons[j].errorGradient = outputs[j] * (1-outputs[j]) * error;
					//errorGradient calculated with Delta Rule: en.wikipedia.org/wiki/Delta_rule
				}
				else
				{
					layers[i].neurons[j].errorGradient = layers[i].neurons[j].output * 
															(1-layers[i].neurons[j].output);
					double errorGradSum = 0;
					for(int p = 0; p < layers[i+1].numberOfNeurons; p++)
					{
						errorGradSum += layers[i+1].neurons[p].errorGradient * 
															layers[i+1].neurons[p].weights[j];
					}
					layers[i].neurons[j].errorGradient *= errorGradSum;
				}	
				for(int k = 0; k < layers[i].neurons[j].numberOfInputs; k++)
				{
					if(i == numberOfHiddenLayers)
					{
						error = desiredOutput[j] - outputs[j];
						layers[i].neurons[j].weights[k] += alpha * layers[i].neurons[j].inputs[k] * 
																	error;
					}
					else
					{
						layers[i].neurons[j].weights[k] += alpha * layers[i].neurons[j].inputs[k] * 
																	layers[i].neurons[j].errorGradient;
					}
				}
				layers[i].neurons[j].bias += alpha * -1 * layers[i].neurons[j].errorGradient;
			}
		}
	}

	//for full list of activation functions
	//see en.wikipedia.org/wiki/Activation_function
	double ActivationFunction(double value)
	{
		return Sigmoid(value);
	}

	double Step(double value) //(aka binary step)
	{
		if(value < 0) return 0;
		else return 1;
	}

	double Sigmoid(double value) //(aka logistic softstep)
	{
    	double k = (double) System.Math.Exp(value);
    	return k / (1.0f + k);
	}
}