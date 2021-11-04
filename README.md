# BetFriend.Api

This is an application to share bets with people. When you create a bet, people can join the bet to say it can not be done.
This application has been done in modular monolithic with TDD and clean architecture. It has a module to bets and an other to users. Each part is independant and can be extract in an other application.
There is a CQRS and event driven architecture approch. When an user is created, an event trigger the member creation with a wallet to allow the creation of bets. The source of truth is an SQL server database. When a bet is inserted, an event trigger the creation this bet in a MongoDB database so that a query can fetch the data without doing any calcul or transformation.
Events are push in azure storage queues and it is azure functions which handle commands in relation of the events.

