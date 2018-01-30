SELECT UE.UserID, E.IDEducationalContent
FROM EducationalContent as E, UserEducationalContent as UE, dbo.[User] as U
WHERE U.UserID = UE.UserID AND 
	  UE.EducationalContentID = E.IDEducationalContent 