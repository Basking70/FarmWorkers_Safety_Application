SELECT FarmID, FarmName, 
					   FarmHouseNumberStreetAddress, FarmCity, 
					   FarmState, FarmCountry, FarmZipCode,
					   FarmLatitute, FarmLongitude,
					   FarmTemperatureMin, FarmTemperatureMax, IsActive,
					   (SELECT U.UserName + ' ' + U.UserLastName + ' Contact: ' + U.UserPhoneNumber 
							FROM dbo.[User] AS U, UserFarm AS UF 
							WHERE U.UserType = 'Farm Owner' AND  
								  U.UserID =  UF.UserID AND 
								  UF.FarmID = F.FarmID AND 
								  UF.IsLatest = '1' ) AS FarmOwner, 
					   (SELECT COUNT(distinct(UF.UserID)) 
						FROM UserFarm AS UF, dbo.[User] AS U 
						WHERE U.UserType = 'Farm Worker' AND U.UserID = UF.UserID AND UF.FarmID = F.FarmID AND UF.IsLatest = '1') AS NumberOfFarmWorkers 
		 FROM Farm 
		 WHERE IsActive = '1' AND UserID = '1' 