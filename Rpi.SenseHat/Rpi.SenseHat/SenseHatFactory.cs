﻿////////////////////////////////////////////////////////////////////////////
//
//  This file is part of Rpi.SenseHat
//
//  Copyright (c) 2017, Mattias Larsson
//
//  Permission is hereby granted, free of charge, to any person obtaining a copy of 
//  this software and associated documentation files (the "Software"), to deal in 
//  the Software without restriction, including without limitation the rights to use, 
//  copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the 
//  Software, and to permit persons to whom the Software is furnished to do so, 
//  subject to the following conditions:
//
//  The above copyright notice and this permission notice shall be included in all 
//  copies or substantial portions of the Software.
//
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
//  INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A 
//  PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
//  HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION 
//  OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE 
//  SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Threading.Tasks;
using RichardsTech.Sensors;
using RichardsTech.Sensors.Devices.HTS221;
using RichardsTech.Sensors.Devices.LPS25H;
using RichardsTech.Sensors.Devices.LSM9DS1;
using Unosquare.RaspberryIO;
using Unosquare.WiringPi;

namespace Emmellsoft.IoT.Rpi.SenseHat
{
    /// <summary>
    /// Factory for creating the ISenseHat object.
    /// </summary>
    public static class SenseHatFactory
	{
		private const byte DeviceAddress = 0x46;

		private static readonly Task<ISenseHat> _getSenseHatTask = GetSenseHatTask();

		/// <summary>
		/// Creates the SenseHat object.
		/// </summary>
		public static Task<ISenseHat> GetSenseHat()
		{
			Pi.Init<BootstrapWiringPi>();
			return _getSenseHatTask;
		}

		private static async Task<ISenseHat> GetSenseHatTask()
		{
			MainI2CDevice mainI2CDevice = CreateDisplayJoystickI2CDevice();

			ImuSensor imuSensor = await CreateImuSensor().ConfigureAwait(false);

			PressureSensor pressureSensor = await CreatePressureSensor().ConfigureAwait(false);

			HumiditySensor humiditySensor = await CreateHumiditySensor().ConfigureAwait(false);

			return new SenseHat(mainI2CDevice, imuSensor, pressureSensor, humiditySensor);
		}

		private static MainI2CDevice CreateDisplayJoystickI2CDevice()
		{
			var device = Pi.I2C.AddDevice(0x46);
		    
			MainI2CDevice main = new MainI2CDevice(device);
			Console.WriteLine("I2C created");
            return main;
		}

		private static async Task<ImuSensor> CreateImuSensor()
		{
			var lsm9Ds1Config = new LSM9DS1Config();

			var imuSensor = new LSM9DS1ImuSensor(
				LSM9DS1Defines.ADDRESS0,
				LSM9DS1Defines.MAG_ADDRESS0,
				lsm9Ds1Config,
				new SensorFusionRTQF());

			await imuSensor.InitAsync().ConfigureAwait(false);
			return imuSensor;
		}

		private static async Task<PressureSensor> CreatePressureSensor()
		{
			var pressureSensor = new LPS25HPressureSensor(LPS25HDefines.ADDRESS0);
			await pressureSensor.InitAsync().ConfigureAwait(false);
			return pressureSensor;
		}

		private static async Task<HumiditySensor> CreateHumiditySensor()
		{
			var humiditySensor = new HTS221HumiditySensor(HTS221Defines.ADDRESS);
			await humiditySensor.InitAsync().ConfigureAwait(false);
			return humiditySensor;
		}
	}
}