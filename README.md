# HawkEye
 Archive server content indexing tool with OCR using Tesseract

## Goal
 The goal of HawkEye is to be a software that scans files of various types and indexes their contents.
 These files can then be filtered and sorted through a web interface or an API.
 If you run HawkEye on your archive server for example, it allows for quick searches of a specific document based on its content.
 In the future, HawkEye should be able to not only store data in a local sqlite database, but also connect to SQL Databases for example.
 Also, an extension feature is planned, so developers can code their own scanners to add support for more file types.
 Developers are also welcomed to code their own scanners and request a merge into HawkEye's official version.

## How HawkEye works
 You will be able to schedule jobs to scan given folders.
 HawkEye implements different scanners to understand various file types. It is also planned to make third-party extensions possible, so developers can code their own scanners to add support for previously unsupported file types.
 HawkEye will then store the scanners' results locally or in a database.
 Those results can then be accessed by the web interface or the API.

## Tesseract (OCR)
 Tesseract is an Optical Character Recognition Software made by Google and developed open-source under the Apache 2.0 license.
 HawkEye will use it to extract text from images.

## Status
 An overview of the current status / the project's milestones can be seen here: https://github.com/Kitt3120/HawkEye/projects
