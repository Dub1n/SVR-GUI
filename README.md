LIBSVM GUI

The Grid Search uses the data stored in [Training Data] and performs cross-validation
on that data up to the length specified. As such it is generally best to use as large a set as possible.
The Best Prediction plot shown uses the General Settings and SVM Options inputted except
for Parameter Selection it uses the C and G values found from its grid search and for
Feature Selection it uses the number of features per attribute found from its grid search.
The Best Prediction graph uses the last 20% of the training data given to the grid search for
testing, and as such is not the best example of how the model would perform.
It is recommended to use the grid search to find the best settings for the data using an
appropriate data set (e.g. the largest available), training the model using these
settings on a portion of the data (around 75%), and finally testing the model with data
not used for training (e.g. the last 25%).

The Training using the [Train Model] button doesn't use cross validation so as to be able to
produce a model that can be used later

For [Number of Features per Attribute] it will still use the number of attributes in the
training data, if hidden the attribute will be assigned 0 features, and if a number
of features is given to an attribute which doesn't exist in the training set it will
count as 0

In this case, Attribute 1 will always be the 'dependant variable', or the one being predicted

Due to arranging the data to be used in training and testing, a small number of values at the start and
end can't be used. Each attribute loses (max number of features + projection - 1) values. The Start and
Length inputs are applied after arranging the values so that the number of values per attribute is the
Length.

The scaling must happen before the data split to ensure that the training and testing sets are scaled
the same amount (Talk about what kind of scaling is being done here, 0 to 1 etc)

Explain how it has to be CSV files

Talk about not putting spaces in folders or file names 

The Test and Train button does the same as Pressing the Train Model, Send to Test, and Test Model Buttons
in sequence so as to save time if using the complete dataset for both. This is only recommended for
Finding good settings and not for producing ideal models or predictions

Draw a flowchart of using the application
Draw a flowchart of what the application does, where data goes etc

The Attribute assigned to column 1 of the converted data will be used as the predicted variable,
regardless of whether it has no features assigned to it for the training instance matrix
This is so that an attribute can be tested for without using it in the instance matrix,
and it is possible that the prediction will be better without it.

Large C values increase computational time, so be careful with large log(C) inputs for the grid search

The step in the data settings sets the frequency of data points being used i.e. if there were one data
point per day and the step was 7 it would use one point per week. The projection, length, and time between features
are calculated based on the step, i.e with the previous example a projection of 2 would predict 2 weeks in advance
and each new feature per attribute would be a week prior to the last, a length of 5 would be using 5 data points
each a week apart. The start point does not change with the step, for instane to use data every Monday or every
Thursday.

Leave Length Black or input 0 to use the maximum possible data

If encountering errors it is possible you will need a newer (or older) MATLAB runtime 

Make sure there are no whitespaces in your application install path, saves directory or file name

When [Scale Data] is selected, the each attribute's data is scaled proportionally between 0 and 1. After being used for
predictions, the testing data and predicted data are scaled up (or down) by the same factor they were scaled down (or up)
This is then used in the plots and in the predicted data file.