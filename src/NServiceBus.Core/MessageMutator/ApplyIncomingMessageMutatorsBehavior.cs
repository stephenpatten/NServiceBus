﻿namespace NServiceBus
{
    using System;
    using NServiceBus.MessageMutator;
    using NServiceBus.Pipeline.Contexts;


    class ApplyIncomingMessageMutatorsBehavior : LogicalMessageProcessingStageBehavior
    {
        public override void Invoke(Context context, Action next)
        {
            var current = context.GetIncomingLogicalMessage().Instance;

            foreach (var mutator in context.Builder.BuildAll<IMutateIncomingMessages>())
            {
                current = mutator.MutateIncoming(current);
                context.GetIncomingLogicalMessage().UpdateMessageInstance(current);
            }

            context.MessageType = context.GetIncomingLogicalMessage().Metadata.MessageType;
            next();
        }
    }
}