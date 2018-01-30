SELECT U.UserPhoneNumber,
	   (SELECT F0.FarmID
	    FROM Farm as F0, UserFarm as UF0 
	    WHERE F0.IsActive = '1' AND
			  F0.FarmID = UF0.FarmID AND
			  UF0.UserID = U.UserID AND
			  UF0.IsLatest = '1') as FarmID
FROM  dbo.[User] as U
WHERE U.IsActive = '1'

SELECT DISTINCT(F.FarmZipCode)
FROM Farm AS F
WHERE IsActive = '1' AND 
	  FarmZipCode IS NOT NULL

	  SELECT F.FarmZipCode, F.FarmCountry
FROM Farm AS F
WHERE IsActive = '1' AND 
	  FarmZipCode IS NOT NULL
GROUP BY F.FarmZipCode, F.FarmCountry
	  