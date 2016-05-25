# iothack-foodforthought

This is a sample app created during our Microsoft IoT Hack in Bangalore (April, 2016). Our solution is detailed below:

There are a lot of organizations where food is served in the cafetaria from the food bins. Our motive is to reduce the amount of leftover food in bins everyday. We follow this process:

1.  We send the RemainingWeight of the food bins periodically from the Weight sensor (simulated) to Azure IoT Hub. Along with this, we send other fields such as DeviceID, MenuItem, BinSrNo, Temperature of food (using temperature sensor on a FEZ hat), etc. All of this data is packaged in JSON and we use the IoT Hub SDK for C# to send data to IoT hub.
2.  This streaming data is captured by a Stream Analytics job which takes the IoT hub as input, and pushes the data to PowerBI for real-time monitoring. Along with this, we push the relevant input parameters to a Blob storage (which will be consumed by our Azure Machine Learning service)
3.  We created an Azure Machine Learning experiment which takes this Blob data as input. Once we have sufficient data, we try to predict the amount of food required on any given day, based on the various parameters of that particular day (these parameters are DayType, MenuItem being prepared and we captured these parameters while sending data to IoT hub as well)

With sufficient data, we'll be able to predict the amount of food that should be prepared (with some accuracy) - and hence, reduce the amount of food being wasted otherwise.
