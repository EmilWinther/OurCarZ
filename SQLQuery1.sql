SELECT SUM(rating) As RatingSum, COUNT(*) As RatingCount FROM Rating 
WHERE UserID = 6

SELECT COUNT(*) As RatingCount FROM Rating
WHERE RouteID = 2 AND UserID = 6

INSERT INTO Rating (rating, UserID, RouteID) 
VALUES ( 3, 7, 2);

SELECT COUNT(*) As RatingCount FROM Rating
WHERE RouteID = 2 AND UserID = 7
