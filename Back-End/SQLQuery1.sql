SELECT F.FarmID, F.FarmName, 
	   F.FarmHouseNumberStreetAddress, F.FarmCity, 
	   F.FarmState, F.FarmCountry, F.FarmZipCode,
	   F.FarmLatitute, F.FarmLongitude,
	   F.FarmTemperatureMin, F.FarmTemperatureMax, F.IsActive
FROM Farm AS F
WHERE IsActive = '1'