SELECT E.IDEducationalContent, E.LinkEducationalContent, E.TypeEducationalContent, E.DescriptionEducationalContent, E.DateEducationalContentCreated, E.IsActive
FROM EducationalContent AS E
LEFT JOIN (SELECT *
		   FROM UserEducationalContent AS UE
		   WHERE UE.UserID = 1) AS SUBUE
ON E.IDEducationalContent = SUBUE.EducationalContentID
WHERE EducationalContentID IS NULL