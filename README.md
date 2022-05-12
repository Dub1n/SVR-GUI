About the Application
=====================

LIBSVM GUI is a Graphic User Interface to easily create timeseries predictions. 
It allows users to organise data, train and use a Support Vector Machine (SVM) 
model, and plot and export the results. It can use epsilon-SVR and nu-SVR models, 
and performs grid searches to find optimal parameters.

The application uses the MATLAB Runtime and LIBSVM package. The LIBSVM Package is 
from https://www.csie.ntu.edu.tw/~cjlin/libsvm/ , this site explains what LIBSVM is and how it is used

It is recommended to read "A Practical Guide to Support Vector Classification" 
written by the developers of LIBSVM for understanding the process of training and 
using an SVM model before using the app 
https://www.csie.ntu.edu.tw/~cjlin/papers/guide/guide.pdf

The developers' FAQ page provides helpful information on how LIBSVM works and 
how to use it https://www.csie.ntu.edu.tw/~cjlin/libsvm/faq.html

To understand what the SVM and Kernel parameters are and how they are used, read 
the paper presenting https://www.csie.ntu.edu.tw/~cjlin/papers/libsvm.pdf


Table of Contents
=================

- Install Instructions
- Using the Application
  - Recommended Procedure
  - Setup
  - Data Management
  - Training Settings
  - SVM Options
  - Grid Search
  - Training
  - Testing
- Preferences
- Arranging Data
- Example Usage
- Info
- Troubleshooting
- Further Developments
- Copyright


Install Instructions
====================

1. The application requires MATLAB Compiler Runtime (MCR) to be installed before 
use. If you already have the MATLAB Compiler toolbox installed you do not need to 
install again, if not you will need to download and install the MATLAB Runtime 
version R2022a for Windows from 
https://uk.mathworks.com/products/compiler/matlab-runtime.html

2. Once installed, run LIBSVM GUI.exe


Using the Application
=====================

## Recommended Procedure

The recommended use of the app is based on the procedure described in 
"A Practical Guide to Support Vector Classification":

1. Select a table with a variable that contains the variable to be predicted
2. Apply preprocessing to the data
3. Decide which variables to use as supporting attributes
4. Convert to a format usable by the application, including scaling the data
5. Perform cross-validation on the data to find the optimal values for C and G
6. Try the available SVM Types and kernels
7. Use the best parameters found to train a training set
8. Test the model

## Setup

When run, the app will create two folders to store data in the same folder the 
.exe is in, these are 'Required_Files' and 'Saves'. You will not need to open or 
use any of the files in 'Required_Files'. Any output files from the app, such as 
the predicted data produced by the model and graphs, will be automatically stored 
in the 'Saves' folder. The Saves folder can be changed to any location on your 
computer by selecting it in the Saves Directory input at the top of the app.

The File Name input is used to set a prefix for all the useful output from the 
app, for example if set to File1 then the predicted data is stored as 
File1_Prediction.

## Data Management

In the Data Management tab, select your timeseries data to be predicted in 
'Data File Source'. Tell the app which column in the csv table is the predicted 
variable (dependant variable) in the 'Predicted' input. 

If any supporting variables are wanted their column in the table is selected in 
the 'Supporting' inputs; the number of columns can be changed by clicking the + 
and - buttons. Up to 6 supporting variables can be used; this is a limitation of 
the app and not LIBSVM.
Select which column of the table gives the time value of the timeseries if there 
is one in the 'Time' input. If there isn't one, this can be left blank and the 
rest of the process is the exact same.

If the selected file's timeseries starts at the top and progresses downwards, 
select 'Top to Bottom', if it starts at the bottom, unselect this checkbox.

If there are labels in the csv file ensure they are in the first row of the table 
and select 'Has Labels'. If there aren't labels uncheck this option and the 
process will run the same. Time and Label features are used solely in the plots.

To apply scaling to the data check the 'Scale Data' checkbox. This is recommended 
as it usually improves prediction accuracy.

Click 'Convert to LIBSVM-Ready File' to produce a file ready to be used by the 
app for training and predicting.

Once a prepared file is produced, it can be sent to be split into two portions 
('Send to Split'), or sent straight to be used in Training or Testing.

Click 'Split' to divide the data into two portions chronologically, the first 
portion is a percentage of the data as determined by the 'Percent Split' input, 
the second portion is the rest. For each portion, click 'Send to Training' to 
send the location of that portion to be used in training the model, and click 
'Send to Testing' to send the location to be used in predicting.

It is recommended to not use data that has been used in training when testing the 
model. This is to reduce overfitting and ensure the model will work with data 
that it has not seen before. To implement this, send a portion of the data 
(around 75-80%) to training and the rest to testing.

## Training Settings

These settings determine which subset of the data is used in the training and 
gridsearch and how it is arranged.

The 'Start' input sets which row of the data file to use as the first set of 
values for the predicted variable.

The 'Length' input sets how many values of the predicted variable are used in 
training. Leave blank or input 0 to use the maximum possible data.

The 'Projection' input determines how far ahead the predicted variable's values 
are from those of the supporting attributes', i.e. a projection of 10 trains 
value n of the predicted variable against values starting at n-10 of the 
supporting attributes' values

The 'Step' input sets the frequency of data points being used i.e. if there were 
one data point per day and the step was 7 it would use one point per week. The 
projection, length, and time between features are calculated based on the step, 
i.e with the previous example a projection of 2 would predict 2 weeks in advance 
and each new feature per attribute would be a week prior to the last, a length of 
5 would be using 5 data points each a week apart. The start point does not change 
with the step.

The 'Number of features per attribute' determines how far the model looks back in 
each attribute's timeseries when training. This can be helpful for producing 
better predictions but should be trialed. The attributes are those determined in 
the Data Management tab, with the dependant variable being Attribute 1, the first 
supporting attribute being Attribute 2, the second Attribute 3 and so on. If 
hidden, the attribute will be assigned 0 features, and if a number of features is 
given for an attribute which doesn't exist in the training data it will not be 
included.

The link button next to 'Number of features per attribute' sets the 'Number of 
features per attribute' in the testing setting to match those of the training 
settings. It is recommended to keep the projection and step the same as well, 
although having a different projection will only shift the true timeseries along 
from the predicted timeseries and give a different accuracy but the predicted 
results remain the same.

## SVM Options

These options determine which type of SVM is used in training the model, the 
Kernel Type used, and the parameters for both.

The 'SVM Type' allows the choice between epsilon-SVR and nu-SVR. Epsilon-SVR is 
recommended. The parameters for each are shown when that SVM type is selected.

The 'Kernel Type' allows the choices of kernel to be used by the SVM. Options 
are: linear, polynomial, radial basis (RBF), and sigmoid. The RBF kernel is 
recommended. The parameters for each are shown when that kernel is selected.

## Grid Search

The Grid Search uses the data stored in 'Training Data' and performs 
cross-validation on that data using the settings in Testing Data and Parameters 
in SVM Options (apart from those being assessed). The Parameter Selection tests 
the model with values of C and G, and outputs the best pair of values for the 
accuracy given by the 'Accuracy' selected in the left panel. The Feature Quantity 
Selection tests the model with different numbers of features for Attributes 1 and 
2 and outputs the best pair for the accuracy selected. Decide when converting the 
file in the Data Management tab which variable to use as the supporting attribute 
to use alongside the predicted variable in the Feature Quantity Selection grid 
search.

A contour plot of the tested values against the accuracy is shown on the right 
panel. Below it is a plot of the true timeseries and predicted timeseries, using 
a portion of the data for training and the rest for testing. The ratio of these 
portions can be set in the preferences, the default is 80%. The settings and 
parameters for this prediction are the same as those for the grid search, except 
the settings or parameters which were searched for, for these it uses the best 
value found by the grid search. Since this prediction only uses the data given in 
'Training Data' it may not the best example of how the model would perform.

Once optimal values are found, click 'Send Values to Settings' to use these 
values in the model training.

It may be better to use the whole data set for this as the use of 
cross-validation reduces the problems associated with using the same data for 
training and testing.

## Training

To train a model, click the 'Train Model' button. It will use the settings in the 
'Training Settings' section and the parameters in the 'SVM Options' section to 
train a model on the data supplied in the 'Training Data' input.

The location of the produced model will be shown in 'Model Output' and can be 
sent to use in prediction with 'Send to Test' next to it.

Information about the model and its generation produced by LIBSVM is displayed 
under the Model Output.

## Testing

To produce predictions for the timeseries stored in the 'Testing Data' location, 
click the 'Test Model' button. 

The process for arranging the data for testing is the same as that for training, 
and the settings 'Number of features per attribute', 'Start', 'Length', 
'Projection', and 'Step' do the same thing here as in the training settings.

A file storing the predicted values is produced and stored in the location shown 
by 'Prediction', and a plot of the predicted timeseries against the actual 
timeseries is displayed on the right panel under 'Prediction Plot'.

The accuracy of the prediction is shown at the bottom of the right panel as the 
Mean Squared Error and Squared Correlation Coefficient.

The 'Train and Test' button does the same as pressing the 'Train Model', 'Send to 
Test', and 'Test Model' buttons in sequence so as to save time.


Preferences
===========

In the Preferences tab options can be found that affect the way the app functions.

'Shrinking' applies a shrinking heuristic to the data to reduce training time. It 
is possible that applying shrinking increases the training time so if training 
takes unexpectedly long see if turning off shrinking.

'Cache Size' sets the amount of memory to be used to store variables. Increasing 
cache size may speed up computation time.

'Grid Search Split' sets how to divide the training data to train and test the 
model for the grid search best prediction.

'Prediction File Type' sets which type of file to export the predicted values, 
either as a csv file or MATLAB array.

A preset saves the current state of the inputs of app as a file stored in 
Sample_Data/Presets and can be loaded to restore that state. This is useful if 
closing the app and opening again without losing the inputs you had been using or 
having to reselect any files to use.

Save a preset with the inputted 'Preset Name' by clicking 'Save'. Load a preset 
by clicking 'Load'

Creating a sample preset with the 'Create' button creates a sample preset that 
can be loaded like a normal preset. Its use is described under Example Usage 
below. The 'Data Location' determines which file is set to the 'Data File Source' 
when the preset is loaded and is automatically set to the sample data described 
below, but can be set to any data file (files with not enough data will not work 
with the example preset's settings).


Arranging Data
==============

LIBSVM requires data in to be in the format of an Instance Matrix and Label
Vector. This is the case for both training and testing. The indexes of the 
timeseries values are arranged as:

  | V |   |1.1|1.2|...|1.i|2.1|2.2|...|2.j|...|n.1|n.2|...|n.k|
  |:-:| - |:-:|:-:|:-:|:-:|:-:|:-:|:-:|:-:|:-:|:-:|:-:|:-:|:-:|
  |p-1|   |-1 |-2 |...|0-i|-1 |-2 |...|0-j|...|-1 |-2 |...|0-k|
  |p+0|   | 0 |-1 |...|1-i| 0 |-1 |...|1-j|...| 0 |-1 |...|1-k|
  |p+1|   | 1 | 0 |...|2-i| 1 | 0 |...|2-j|...| 1 | 0 |...|2-k|
  |p+2|   | 2 | 1 |...|3-i| 2 | 1 |...|3-j|...| 2 | 1 |...|3-k|
  |p+3|   | 3 | 2 |...|4-i| 3 | 2 |...|4-j|...| 3 | 2 |...|4-k|
  |:|&ensp;&ensp;&ensp;|:|:|:|:|:|:|: | : | : | : | : | : | : |

  key:	
V: Label Vector values from Attribute 1;
1.1: First feature from Attribute 1's values;
1.2: Second feature from Attribute 1's values;
1.i: ith feature from Attribute 1's values;
2.1: First feature from Attribute 2's values;
2.2: Second feature from Attribute 2's values;
2.j: jth feature from Attribute 2's values;
n.1: First feature from Attribute n's values;
n.2: Second feature from Attribute n's values;
n.k: kth feature from Attribute n's values;
p: projection, how far ahead the label vector is from the instance matrix;
s: step;
a: start

Each index here includes adding the largest number of features assigned to an 
attribute called m (max of i, j, ..., k), and the start value.
i.e., 1 would be (1 + m + start)

When including Step, each index value is multiplied by the step, and then 
subtracts the start value times the step - 1
i.e., x -> x*step - start(step - 1)

A complete version would be:

  |V|&ensp;&ensp;&ensp;&ensp;&ensp;|n.1|n.2|...|n.i   |
  |:--------:| - |:------:|:------:|:-----:|:--------:|
  |a+(m+p-1)s|   |a+(m-1)s|a+(m-2)s|  ...  |a+(m-i+0)s|
  |a+(m+p+0)s|   |a+(m+0)s|a+(m-1)s|  ...  |a+(m-i+1)s|
  |a+(m+p+1)s|   |a+(m+1)s|a+(m+0)s|  ...  |a+(m-i+2)s|
  |a+(m+p+2)s|   |a+(m+2)s|a+(m+1)s|  ...  |a+(m-i+3)s|
  |a+(m+p+3)s|   |a+(m+3)s|a+(m+2)s|  ...  |a+(m-i+4)s|

The number of rows created is equal to the input Length.

For example, if using the values Start = 10, Length = 6, Projection = 2,
and Step = 7, applying this arrangement two attributes, each containing the 
integers 1 to 100 (1:1:100), with the first attribute assigned 2 features and
the second assigned 4 features, the result would be:

  | V  |    |1.1 |1.2 |2.1 |2.2 |2.3 |2.4 |
  |:--:|:--:|:--:|:--:|:--:|:--:|:--:|:--:|
  | 45 |    | 31 | 24 | 31 | 24 | 17 | 10 |
  | 52 |    | 38 | 31 | 38 | 31 | 24 | 17 |
  | 59 |    | 45 | 38 | 45 | 38 | 31 | 24 |
  | 66 |    | 52 | 45 | 52 | 45 | 38 | 31 |
  | 73 |    | 59 | 62 | 59 | 52 | 45 | 38 |
  |80|&ensp;&ensp;&ensp;|66|59|66|59|52|45|

The Label Vector is displayed on the left and the instance matrix is displayed on 
the right. This shows which of the attribute's values are used with the 
predicted attribute's values.

As shown, for each value in the Instance Matrix, the Label Vector provides the
values of each attribute looking back by a defined range.


Example Usage
=============

This is an example usage to test the application functions, it requires the 
data file 39001_data.csv which comes with the application in the Sample_Data 
folder.

This data was taken from CAMELS GB and is intended for demonstration purposes.
See https://essd.copernicus.org/articles/12/2459/2020/

1. Go to the Preferences tab and click the 'Create' button under 
   'Create Sample Preset'
2. Click the 'Load Preset' button just above it and select the SamplePreset file
3. Go to the Data Management tab and click 'Convert to LIBSVM-Ready File'
4. Once complete, click 'Send to Split' beneath the Prepared File location
5. Click the 'Split' button
6. For the First Portion, click 'Send to Training'
7. For the Second Portion, click 'Send to Testing'
8. Go to the Test and Train tab and click 'Run Search' under Grid Search
9. Click 'Send Values to Settings' under Grid Search Results
10. Click 'Train and Test' under Training (or 'Train Model', 'Send to Test', then 
    'Test Model')


Info
====

When using the app and unsure of what to use in an input, hover the input label 
to see what it does.

The application does not apply pre-processing to any data used except for 
scaling. This is because it is a very case-by-case process and should be done 
before using the application if needed. For example, if any data is missing in 
one or more attribute it will be detrimental to the prediction capacities of the 
model, and as such should be removed or substituted in an appropriate manner.

Only .csv files are accepted as input for the conversion to a LIBSVM-ready file. 
This is because it is a universal file format and easily used by MATLAB.

The scaling applied in the conversion process scales each attribute between 0 and 1. 
This is very helpful for training and predicting as it both speeds up the 
computation time and stops any one attribute outweighing another in training the 
model. Scaling can be turned off before conversion. After being used for 
predictions, the testing data and predicted data are scaled up (or down) by the 
same factor they were scaled down (or up). This is then used in the plots and in 
the predicted data file.

The scaling must happen before the data split to ensure that the training and 
testing sets are scaled the same amount.

The Training using the 'Train Model' button doesn't use cross validation in order 
to produce a model that can be used later.

The Attribute assigned to column 1 of the converted data will be used as the 
predicted variable, regardless of whether it has no features assigned to it for 
the training instance matrix. This is so that an attribute can be predicted 
without using it in the instance matrix, and it is possible that the prediction 
will be better without it.

Due to arranging the data to be used in training and testing, a small number of 
values at the start and end can't be used. Each attribute loses (max number of 
features + projection - 1) values. This has been optimised to use the maximum 
number of values possible.


Troubleshooting
===============

Do not close the plots before they automatically close, otherwise they might not 
be able to be saved or displayed in the app

Try to store the application folder in a path that does not contain any 
whitespaces (spaces in the folder names) as this may cause issues.

Do not delete any of the .dll files from the folder the app is stored in or 
else it won't work.

Do not delete anything from the 'Required_Files' folder during the program 
running.

Make sure to keep 'LIBSVM GUI.exe' in the same folder as the .dll files that 
came with it.

If encountering errors, it is possible you will need a newer (or older) MCR. 
The version of MCR the app was built for is 2022a (9.12).

Make sure there are no whitespaces in the saves directory or file name.

If there is an error with training, testing, or grid search, it may be because 
there aren't enough values in the data as requested by the Training Settings 
inputs.

If the app crashes it may be because too many programs were running at once, 
allow each to finish before starting another if this persists.

The application may need to be run as administrator or whitelisted from a 
firewall to edit files if in a protected folder.

Large C values increase computational time, so be careful with large log(C) 
inputs for the grid search.

Should a process take too long: save a preset, close the app, reopen it, and 
load the saved preset.


Furter Developments
===================

Improvements and adjustments in any and every way are very welcome. Here are the
changes that I would make with enough time and skill

- Check if all the number inputs are not valid and not empty
- Force quit function
- Allow csv files and not just MATLAB files to be used in training and predicting
- Make cross-platform 
- Allow files to be imported that have incomplete data i.e., if some columns have fewer data points than others
- Remake so that it either doesn't use MATLAB at all using .NET or C# wrappers of LIBSVM
- Allow an unrestricted number of attributes be used in testing and training
- Make the plots interactive
- Output more informative error information, such as "not enough data for requested length"
- Implement the .NET or C# wrappers of LIBSVM if possible
- Add a preview of the exported files to the Data Management window
- Add LIBSVM output text to drop-down
- Add a way to report errors helpfully, either through app or elsewhere
- Add options to increase readability i.e., increased font size, high contrast.
- Add an option to run through the grid search automatically and iteratively
- Make the interface scalable, including for full screen
- Allow forecasting of data without comparison to a timeseries input


LIBSVM Copyright
================

Chih-Chung Chang and Chih-Jen Lin, LIBSVM: a library for support vector machines, 2001. Software available at http://www.csie.ntu.edu.tw/~cjlin/libsvm.

Copyright (c) 2000-2021 Chih-Chung Chang and Chih-Jen Lin
All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions
are met:

1. Redistributions of source code must retain the above copyright
notice, this list of conditions and the following disclaimer.

2. Redistributions in binary form must reproduce the above copyright
notice, this list of conditions and the following disclaimer in the
documentation and/or other materials provided with the distribution.

3. Neither name of copyright holders nor the names of its contributors
may be used to endorse or promote products derived from this software
without specific prior written permission.


THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
``AS IS'' AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
A PARTICULAR PURPOSE ARE DISCLAIMED.  IN NO EVENT SHALL THE REGENTS OR
CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

CAMELS-GB
=========

Contains data supplied by Natural Environment Research Council.

Coxon, G.; Addor, N.; Bloomfield, J.P.; Freer, J.; Fry, M.; Hannaford, J.; 
Howden, N.J.K.; Lane, R.; Lewis, M.; Robinson, E.L.; Wagener, T.; Woods, R. 
(2020). Catchment attributes and hydro-meteorological timeseries for 671 
catchments across Great Britain (CAMELS-GB) NERC Environmental Information 
Data Centre. https://doi.org/10.5285/8344e4f3-d2ea-44f5-8afa-86d2987543a9

Earth Syst. Sci. Data, 12, 2459–2483, 2020
https://doi.org/10.5194/essd-12-2459-2020
© Author(s) 2020. This work is distributed under
the Creative Commons Attribution 4.0 License.
