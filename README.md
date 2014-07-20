# Dice Roller for Windows Phone 8 #

This is a dice roller application for tabletop role playing games, such as _Dungeons and Dragons_, _Exalted_, and _Legend_. It is designed to handle common dice from D4 through to D%. Which dice are available should be made configurable in due course.

### Features ###
 - Customize which dice are displayed and used (avoiding clutter)
 - History
 - Favourites
 - Consistent UI with Windows Phone 8
 - Rapid familiarization and use

## NuGet Packages ##

Currently this project uses NuGet packages which are not included in the GIT repository. Building the solution in Visual Studio should automatically install any missing packages.

## Git Flow ##

This project uses the development model set out in http://nvie.com/posts/a-successful-git-branching-model. The conventions set out in this article, such as develop and master branches, and naming release-*, and so on are followed exactly.

## Version History ##

### In Development

 - Updated application icon (all locations)
 - Updated dice icons in pick view
 - Added watermarks to Favourites, History and Results views
 - Fix landscape pick tile size
 - Updated dependencies

### **Version 2.1.1** - 9 February 2014

 - Fix for version update where previous version was not an explicit 'update from' version

### **Version 2.1.0** - 9 February 2014

 - Application state is saved when changes are made, rather than only on deactivate/close
 - Mechanism to handle updates which affect stored data
 - Optimized what data is saved

### **Version 2.0.1** - 9 February 2014

 - Fix for save changes to favourites

### **Version 2.0.0** - 9 February 2014

 - Favourites feature (missing some key features such as reordering)
 - Persistence between application runs
 - Localization
   - English (United Kingdom)
   - English (United States)
 - Usage of 'Tilt Effect' on expected controls
 - Styling improvements
 - Bug fixes

### **Version 1.0.1** - 8 December 2013

 - Improvements to localization strategy and coverage
 - Minor styling improvements

### **Version 1.0** - 18 November 2013

 - Pick and History pivot tabs, supporting click and hold-to-picker to set pool
 - Info page show on roll, and from History with individual results