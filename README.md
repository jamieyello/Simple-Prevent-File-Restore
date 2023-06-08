# Simple Prevent File Restore
A simple file restore prevention tool that securely wipes data using cryptographically random data.

Windows is the only supported platform currently. Other OS's can be added easily.

<img width="635" alt="image" src="https://github.com/jamieyello/Simple-Prevent-File-Restore/assets/10054829/b24db2a6-ac8c-4f45-80c9-ab7b4c18bf86">

## Why?
When an OS deletes a file, it's typically not deleted fully. It's space is just marked as available to use, and any data will remain there until another file overwrites it. This can lead to data theft. For example, say you discard your old hard drive you no longer want. If thrown into the trash or recycled, it will very likely be picked out, where anyone down the line will have full access to anything that was on that drive.

## What does this do?
This small program simply creates one big file on a specified drive containing cryptographically random data. This prevents any (once deleted) data from being recovered using any conventional file recovery tool. Doing multiple passes will ensure any data is completely beyond recovery by even the most capable actors.

The code is made as simple as possible for better transparency.

## How should I use it?
While a hard drive has virtually unlimited read/write cycles, an SSD only has about 100,000 in total. Keep this in mind regarding how often you use this tool.
