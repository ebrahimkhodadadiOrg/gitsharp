UPDATE products
SET price = ROUND(price * 1.5);
گرد کردن نرمال
UPDATE products
SET price = CEIL(price * 1.5);
گرد کردن به سمت بالا 
UPDATE products
SET price = FLOOR(price * 1.5);
گرد کردن به سمت پایین