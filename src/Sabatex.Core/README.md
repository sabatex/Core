## Sabatex base common definitions for all libraries
This library contains common definitions and utilities that are used across all Sabatex libraries. It is designed to provide a shared foundation for the Sabatex ecosystem, ensuring consistency and reusability of code.

[![MIT License](https://img.shields.io/badge/license-MIT-red.svg)](https://github.com/sabatex/Extensions/blob/master/LICENSE.TXT)
[![NuGet Version](https://img.shields.io/nuget/v/Sabatex.Core?label=version&logo=nuget&style=social&label=)](https://www.nuget.org/packages/Sabatex.Core)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Sabatex.Core?color=%232694F9&label=downloads&logo=nuget&style=social)](https://www.nuget.org/packages/Sabatex.Core)


 - #Observable object with INotifyPropertyChanged implemented

 - #Char extensions     UpperKeyToRus(),char.UpperKeyToUkr - recode englesh key to Russian or Ukrainian

 - #DateTime extensions: BeginOfDay(),EndOfDay(),BeginOfWeek(),EndOfWeek(),BeginOfMonth(),
                        EndOfMonth(),BeginOfYear(),EndOfYear(),Quarter(),BeginOfQuarter(),
                        EndOfQuarter(),WeekOfYear()
 
 - #Enum extensions:   Description() - get enum description attribute
                      GetEnumDisplayName()  - get array Tuple<name,description> 
                      GetEnumListWithDescription() - get array Tuple<enum,description>

 - #MemberInfo extensions: GetCustomAttribute(Type attributeType) 

 - #Period Represents a dates period (start - end)
 
 
