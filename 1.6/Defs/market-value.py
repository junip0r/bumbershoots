#!/bin/python

nothing = 0
cloth = 1.5
wood = 1.2
steel = 1.9
markup = 2

def value(qty1, value1, qty2, value2):
  cost = qty1 * value1 + qty2 * value2
  cost *= markup
  pad = 5 - cost % 5 if cost % 5 > 0 else 0
  cost += pad
  return cost

parasol  = value(15, wood,    25, cloth)
umbrella = value(15, steel,   30, cloth)
golf     = value(20, steel,   40, cloth)
fashion  = value(0,  nothing, 40, cloth) + golf

print("parasol", parasol)
print("umbrella", umbrella)
print("golf", golf)
print("fashion", fashion)
