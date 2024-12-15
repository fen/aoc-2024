# Day13 notes

Cramer's rule is an elegant mathematical solution to solve systems of two linear equations with two variables. It is
applicable in our case because we have two equations:

1. $( a \cdot A_x + b \cdot B_x = P_x )$
2. $( a \cdot A_y + b \cdot B_y = P_y )$

where $( a )$ (number of presses of Button A) and $( b )$ (number of presses of Button B) are the unknowns we need to
compute, assuming $( A_x )$, $( A_y )$, $( B_x )$, $( B_y )$, $( P_x )$, and $( P_y )$ are known constants.

The benefit of using Cramer's rule is that it computes the solution directly using determinants, with the following
formulas:

$$
[ a = \frac{\text{det}(Px, Bx; Py, By)}{\text{det}(Ax, Bx; Ay, By)} ]
$$

$$
[ b = \frac{\text{det}(Ax, Px; Ay, Py)}{\text{det}(Ax, Bx; Ay, By)} ]
$$

Where `det` is the determinant of a $2×2$ matrix. For a $2×2$ matrix, you can calculate the determinant as:

$$
[ \text{det}(a, b; c, d) = (a \cdot d) - (b \cdot c) ]
$$
