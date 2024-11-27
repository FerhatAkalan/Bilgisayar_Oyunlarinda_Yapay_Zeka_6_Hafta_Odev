using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neuron_sc {

	public int numberOfInputs;
	public double bias;
	public double output;
	public double errorGradient;
	public List<double> weights = new List<double>();
	public List<double> inputs = new List<double>();
	// Nöron sınıfı, belirli sayıda giriş alır ve bu girişleri işlemek için gerekli ağırlıkları başlatır
	public Neuron_sc(int numberOfInputs)
	{
		bias = UnityEngine.Random.Range(-1.0f, 1.0f);
		this.numberOfInputs = numberOfInputs;
		for(int i = 0; i < numberOfInputs; i++)
			weights.Add(UnityEngine.Random.Range(-1.0f, 1.0f));
	}
}